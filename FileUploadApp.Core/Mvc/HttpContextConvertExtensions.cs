using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.StreamAdapters;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Core
{
    public static class HttpContextConvertExtensions
    {
        public static async Task<IEnumerable<Upload>> AsUploadFilesEventAsync(this HttpContext httpContext
            , IContentTypeTestUtility contentTypeTestUtility
            , CancellationToken cancellationToken = default)
        {
            var form = await httpContext.Request.ReadFormAsync(cancellationToken);
            var filesCollection = new List<Upload>();
            var number = 0U;

            foreach (var f in form.Files)
            {
                if (contentTypeTestUtility.IsAllowed(f.ContentType))
                    filesCollection.Add(new Upload(
                        id: Guid.NewGuid(),
                        previewId: Guid.NewGuid(),
                        num: number++,
                        name: f.FileName,
                        contentType: f.ContentType,
                        streamAdapter: new FormFileStreamAdapter(new FormFileDecorator(f))
                    ));
            }

            return filesCollection.ToArray();
        }
    }
}
