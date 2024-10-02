using System;
using System.Collections.Generic;
using System.Linq;

using FileUploadApp.Application.Uploading.Commands;
using FileUploadApp.Domain;
using FileUploadApp.Domain.Raw;
using FileUploadApp.Interfaces;

namespace FileUploadApp.Application;

public static class UploadRequestExtensions
{
    public static IEnumerable<Upload> AsUploads(this UploadRequest uploadRequest, IContentTypeTestUtility contentTypeTestUtility)
        => uploadRequest.Files?
            .AsFileDescriptors(contentTypeTestUtility) ?? [];

    public static IEnumerable<DownloadUri.Command> AsDownloadUriQueries(this UploadRequest uploadRequest, Action<string>? onError = default)
        => uploadRequest.Links?
            .AsOrderedUriEnumerable(onError: onError)
            .Select(x => new DownloadUri.Command(x.Item1, x.Item2)) ?? [];
}
