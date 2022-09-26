using FileUploadApp.Domain;
using FileUploadApp.Domain.Raw;
using FileUploadApp.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Features.Services;

public class ContentDownloader : IContentDownloader<DownloadUriResponse>
{
    private const string UserAgentField = "User-Agent";

    private readonly AppConfiguration _configuration;
    private readonly HttpClientHandler _sharedHandler;
    private readonly Uri _address;

    public ContentDownloader(AppConfiguration configuration, HttpClientHandler sharedHandler, Uri address)
    {
        _configuration = configuration;
        _sharedHandler = sharedHandler;
        _address = address;
    }

    public async Task<DownloadUriResponse> DownloadAsync(CancellationToken cancellationToken = default)
    {
        using var client = GetClient();
        using var message = await client.GetAsync(_address, cancellationToken).ConfigureAwait(false);
        var contentType = message.Content.Headers.ContentType;
        var data = await message.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);

        return new DownloadUriResponse(_address, contentType.MediaType, data);
    }

    private HttpClient GetClient()
    {
        var client = new HttpClient(_sharedHandler, disposeHandler: false);

        client.DefaultRequestHeaders.Add(UserAgentField, _configuration.DefaultUserAgent);

        return client;
    }
}
