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
        }

        public static async Task<FileUploadEvent> AssumeAsFilesRequestEvent(this HttpContext httpContext)
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

            return new FileUploadEvent(filesCollection);
        }

        public static async Task<UploadRequestEvent> AssumeAsUploadRequestEvent(this HttpContext httpContext, IDeserializer deserializer)
        {
            var request = httpContext.Request.EnableRewind();
            var ctx = await request.DesirializeAsync<UploadRequest>(deserializer).ConfigureAwait(false);
            var files = ctx.Files.AsFileDesciptors();

            return new UploadRequestEvent(files.ToArray(), ctx.Links);
        }

        public static IEnumerable<FileDescriptor> AsFileDesciptors(this IEnumerable<Base64FilePayload> plds)
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

        public static async Task<PlainTextRequestEvent> AssumeAsPlaintTextRequestEvent(this HttpContext httpContext)
        {
            var request = httpContext.Request.EnableRewind();
            var text = await request.ReadAsReadonlyMemoryAsync().ConfigureAwait(false);

            return new PlainTextRequestEvent(text);
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
