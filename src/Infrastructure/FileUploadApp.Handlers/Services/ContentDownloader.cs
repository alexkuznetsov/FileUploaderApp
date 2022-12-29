using FileUploadApp.Domain.Raw;
using FileUploadApp.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Features.Services;

public class ContentDownloader : IContentDownloader<DownloadUriResponse>
{
    public const string UserAgentField = "User-Agent";
    private readonly IHttpClientFactory _httpClientFactory;

    public ContentDownloader(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;

    public async Task<DownloadUriResponse> DownloadAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient();
        using var message = await client.GetAsync(uri, cancellationToken).ConfigureAwait(false);
        var contentType = message.Content.Headers.ContentType;
        var data = await message.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        return new DownloadUriResponse(uri, contentType.MediaType
            , new StreamAdapters.CommonStreamStreamAdapter(data));
    }
}
