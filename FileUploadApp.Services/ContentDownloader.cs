using FileUploadApp.Core.Configuration;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Services
{
    public class ContentDownloader : IContentDownloader<DownloadUriResponse>
    {
        private static readonly string UserAgentField = "User-Agent";

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
            using (var client = GetClient())
            {
                var message = await client.GetAsync(_address, cancellationToken);
                var content = message.Content;
                var contentType = message.Content.Headers.ContentType;
                var data = await content.ReadAsByteArrayAsync();

                return new DownloadUriResponse(_address, contentType.MediaType, data.AsMemory());
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient(_sharedHandler, disposeHandler: false);

            client.DefaultRequestHeaders.Add(UserAgentField, _configuration.DefaultUserAgent);

            return client;
        }
    }
}
