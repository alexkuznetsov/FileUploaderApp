﻿using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.StreamAdapters;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var number = 0U;

            return (
                from f in form.Files
                where contentTypeTestUtility.IsAllowed(f.ContentType)
                select new Upload(
                    id: Guid.NewGuid(),
                    previewId: Guid.NewGuid(),
                    num: number++,
                    name: f.FileName,
                    contentType: f.ContentType,
                    streamAdapter: new FormFileStreamAdapter(new FormFileDecorator(f))
                )).ToArray();
        }
    }
}