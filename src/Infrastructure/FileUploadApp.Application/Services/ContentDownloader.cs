using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Domain.Raw;
using FileUploadApp.Interfaces;

namespace FileUploadApp.Application.Services;

internal sealed class ContentDownloader(HttpClient httpClient)
    : IContentDownloader<DownloadUriResponse>
{
    public const string UserAgentField = "User-Agent";

    public async Task<DownloadUriResponse> DownloadAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        using var message = await httpClient
            .GetAsync(uri, cancellationToken)
            .ConfigureAwait(false);

        var ms = new MemoryStream();
        var contentType = message.Content.Headers.ContentType;

        await message.Content.CopyToAsync(ms, cancellationToken)
            .ConfigureAwait(false);

        ms.Seek(0, SeekOrigin.Begin);

        return new DownloadUriResponse(uri, contentType?.MediaType ?? MimeConstants.OctetStreamMime, ms);
    }
}
