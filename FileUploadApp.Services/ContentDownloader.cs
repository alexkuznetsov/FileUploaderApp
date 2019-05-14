using FileUploadApp.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FileUploadApp.Services
{
    public class ContentDownloader : IContentDownloader
    {
        private const string DefaultUserAgent =
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36 OPR/54.0.2952.46";

        private readonly HttpClientHandler _sharedHandler;
        private readonly Uri _address;

        public ContentDownloader(HttpClientHandler sharedHandler, Uri address)
        {
            _sharedHandler = sharedHandler;
            _address = address;
        }

        public async Task<byte[]> DownloadAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            using (var client = GetClient())
            {
                var message = await client.GetAsync(_address, cancellationToken);

                return await message.Content.ReadAsByteArrayAsync();
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient(_sharedHandler, disposeHandler: false);

            client.DefaultRequestHeaders.Add("User-Agent", DefaultUserAgent);

            return client;
        }
    }
}
