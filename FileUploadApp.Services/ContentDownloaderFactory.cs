using System;
using System.Net.Http;

namespace FileUploadApp.Services
{
    public class ContentDownloaderFactory
    {
        private readonly HttpClientHandler delegatingHandler;

        public ContentDownloaderFactory(HttpClientHandler delegatingHandler)
        {
            this.delegatingHandler = delegatingHandler;
        }

        public ContentDownloader Create(Uri uri) => new ContentDownloader(delegatingHandler, uri);
    }
}
