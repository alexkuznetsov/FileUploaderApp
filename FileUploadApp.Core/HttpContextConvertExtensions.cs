using FileUploadApp.Domain;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Events;
using FileUploadApp.Interfaces;
using FileUploadApp.StreamAdapters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Core
{
    internal static class HttpContextConvertExtensions
    {
        private class FormFileDecorator : IFormFileDecorator
        {
            private readonly IFormFile formFile;

            public FormFileDecorator(IFormFile formFile)
            {
                this.formFile = formFile;
            }

            public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
                => formFile.CopyToAsync(target, cancellationToken);

            public Stream GetStream() => formFile.OpenReadStream();
        }

        public static async Task<IEnumerable<GenericEvent>> AssumeAsUploadRequestEvents(this HttpContext httpContext, IDeserializer deserializer)
        {
            var request = httpContext.Request.EnableRewind();
            var rq = await request.DesirializeAsync<UploadRequest>(deserializer).ConfigureAwait(false);
            var events = new List<GenericEvent>();

            return ConvertIfAny(rq?.Files?.AsFileDesciptors(), (f) => new ProcessFileDescriptorEvent(f.ToArray()))
                .Concat(ConvertIfAny(rq?.Links, (f) => new ProcessImageUriEvent(f.ToArray())))
                .ToArray();
        }

        private static IEnumerable<GenericEvent> ConvertIfAny<TRequest, TEvent>(IEnumerable<TRequest> source, Func<IEnumerable<TRequest>, TEvent> builder)
            where TEvent : GenericEvent
        {
            var isFilesNotEmpty = source != null && source?.Count() > 0;

            if (isFilesNotEmpty)
            {
                yield return builder(source);
            }
        }

        public static async Task<IEnumerable<GenericEvent>> AssumeAsFilesRequestEvents(this HttpContext httpContext)
        {
            var form = await httpContext.Request.ReadFormAsync();
            var filesCollection = new List<FileDescriptor>();
            var number = 0U;

            foreach (var f in form.Files)
            {
                filesCollection.Add(new FileDescriptor
                    (
                        num: number++,
                        contentType: f.ContentType,
                        name: f.FileName,
                        stream: new FormFileStreamAdapter(new FormFileDecorator(f))
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

        private static IEnumerable<FileDescriptor> AsFileDesciptors(this IEnumerable<Base64FilePayload> plds)
        {
            var i = 0U;
            foreach (var p in plds)
            {
                var bytea = Convert.FromBase64String(p.Base64).AsMemory();
                yield return new FileDescriptor(
                    num: i++,
                    name: p.Name,
                    contentType: string.Empty,
                    stream: new ByteaStreamAdapter(bytea));
            }
        }

        private static async Task<ReadOnlyMemory<char>> ReadAsReadonlyMemoryAsync(this HttpRequest request)
        {
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                var result = await reader.ReadToEndAsync().ConfigureAwait(false);
                return result.AsMemory();
            }
        }

        private static async Task<string> ReadAsPlainTextAsync(this HttpRequest request)
        {
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                return await reader.ReadToEndAsync().ConfigureAwait(false);
            }
        }

        private static async Task<T> DesirializeAsync<T>(this HttpRequest request, IDeserializer deserializer)
        {
            var content = await request.ReadAsPlainTextAsync().ConfigureAwait(false);

            return await deserializer.DeserializeAsync<T>(content).ConfigureAwait(false);
        }
    }
}
