using FileUploadApp.Domain.Raw;
using FileUploadApp.Interfaces;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Features.Services;

public class ContentDownloader : IContentDownloader<DownloadUriResponse>
{
    public const string UserAgentField = "User-Agent";

    private readonly HttpClient _httpClient;

    public ContentDownloader(HttpClient httpClient) => 
        _httpClient = httpClient;

    public async Task<DownloadUriResponse> DownloadAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        using var message = await _httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);

        var ms = new MemoryStream();
        var contentType = message.Content.Headers.ContentType;
        
        await message.Content.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);

        ms.Seek(0, System.IO.SeekOrigin.Begin);

        return new DownloadUriResponse(uri, contentType.MediaType, ms);
    }
}
