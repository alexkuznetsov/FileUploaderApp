using FileUploadApp.Domain;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileUploadApp.Core
{
    public static class UploadRequestExtensions
    {
        public static IEnumerable<Upload> AsUploads(this UploadRequest uploadRequest, IContentTypeTestUtility contentTypeTestUtility)
            => uploadRequest.Files?
                .AsFileDescriptors(contentTypeTestUtility) ?? new Upload[] { };

        public static IEnumerable<DownloadUriQuery> AsDownloadUriQueries(this UploadRequest uploadRequest, Action<string> onError = default)
            => uploadRequest.Links?
                .AsOrderedUriEnumerable(onError: onError)
                .Select(x => new DownloadUriQuery(x.Item1, x.Item2)) ?? new DownloadUriQuery[] { };
    }
}
