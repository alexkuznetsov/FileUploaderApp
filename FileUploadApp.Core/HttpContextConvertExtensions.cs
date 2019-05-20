using FileUploadApp.Core.Infrastructure;
using FileUploadApp.Domain;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Events;
using FileUploadApp.Interfaces;
using FileUploadApp.StreamAdapters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadApp.Core
{
    public static class HttpContextConvertExtensions
    {
        public static async Task<IEnumerable<GenericEvent>> AssumeAsUploadRequestEvents(this HttpContext httpContext)
        {
            var rq = await httpContext.DeserializeRequestAsync<UploadRequest>().ConfigureAwait(false);

            return new UploadRequestEventBuilder(rq).BuildEvents();
        }

        public static async Task<IEnumerable<GenericEvent>> AssumeAsFilesRequestEvents(this HttpContext httpContext)
        {
            var form = await httpContext.Request.ReadFormAsync();
            var filesCollection = new List<FileDescriptor>();
            var number = 0U;

            foreach (var f in form.Files)
            {
                filesCollection.Add(new FileDescriptor(
                    id: Guid.NewGuid(),
                    number: number++,
                    name: f.FileName,
                    contentType: f.ContentType,
                    streamAdapter: new FormFileStreamAdapter(new FormFileDecorator(f))
                ));
            }

            return new[] { new ProcessFileDescriptorEvent(filesCollection.ToArray()) };
        }

        public static async Task<IEnumerable<GenericEvent>> AssumeAsPlaintTextRequestEvents(this HttpContext httpContext)
        {
            var request = httpContext.Request.EnableRewind();
            var text = await request.ReadAsReadonlyMemoryAsync().ConfigureAwait(false);

            return new[] { new PlainTextRequestEvent(text) };
        }

        private static async Task<ReadOnlyMemory<char>> ReadAsReadonlyMemoryAsync(this HttpRequest request)
        {
            var result = await request.ReadAsPlainTextAsync().ConfigureAwait(false);

            return result.AsMemory();
        }

        private static async Task<T> DeserializeRequestAsync<T>(this HttpContext httpContext)
        {
            var deserializer = httpContext.RequestServices.GetRequiredService<IDeserializer>();
            var request = httpContext.Request.EnableRewind();
            var content = await request.ReadAsPlainTextAsync().ConfigureAwait(false);

            return deserializer.Deserialize<T>(content);
        }

        private static async Task<string> ReadAsPlainTextAsync(this HttpRequest request)
        {
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                return await reader.ReadToEndAsync().ConfigureAwait(false);
            }
        }
    }
}
