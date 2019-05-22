using FileUploadApp.Domain;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using System.Collections.Generic;
using System.Linq;

namespace FileUploadApp.Core
{
    public static class UploadRequestExtensions
    {
        public static IEnumerable<Upload> AsUploads(this UploadRequest uploadRequest, IContentTypeTestUtility contentTypeTestUtility)
            => uploadRequest.Files?
            .AsFileDesciptors(contentTypeTestUtility) ?? new Upload[] { };

        public static IEnumerable<DownloadUriQuery> AsDownloadUriQueries(this UploadRequest uploadRequest)
            => uploadRequest.Links?
                .AsOrderedUriEnumerable()
                .Select(x => new DownloadUriQuery(x.Item1, x.Item2)) ?? new DownloadUriQuery[] { };
    }
}
