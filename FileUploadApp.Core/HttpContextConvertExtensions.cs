using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.StreamWrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Core
{
    internal static class HttpContextConvertExtensions
    {
        private class FormFileWrapper : IFormFileWrapper
        {
            private readonly IFormFile formFile;

            public FormFileWrapper(IFormFile formFile)
            {
                this.formFile = formFile;
            }

            public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
                => formFile.CopyToAsync(target, cancellationToken);

            public Stream GetStream()
                => formFile.OpenReadStream();
        }

        public static async Task<IEnumerable<FormFileDescriptor>> AssumeAsFilesRequestEvent(this HttpContext httpContext)
        {
            var form = await httpContext.Request.ReadFormAsync();
            var filesCollection = new List<FormFileDescriptor>();

            foreach (var f in form.Files)
            {
                filesCollection.Add(new FormFileDescriptor
                (
                    contentType: f.ContentType,
                    name: f.FileName,
                    stream: new FormFileStreamWrapper(new FormFileWrapper(f))
                ));
            }

            return filesCollection;
        }

        public static async Task<UploadRequest> AssumeAsUploadRequestEvent(this HttpContext httpContext, IDeserializer deserializer)
        {
            var request = httpContext.Request.EnableRewind();

            return await request.DesirializeAsync<UploadRequest>(deserializer).ConfigureAwait(false);
        }

        public static async Task<string> AssumeAsPlaintTextRequestEvent(this HttpContext httpContext)
        {
            var request = httpContext.Request.EnableRewind();

            return await request.ReadAsPlainTextAsync().ConfigureAwait(false);
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
