using FileUploadApp.Domain;
using System;
using System.Net.Http;

namespace FileUploadApp.Services
{
    public class ContentDownloaderFactory
    {
        private readonly HttpClientHandler delegatingHandler;
        private readonly AppConfiguration configuration;

        public ContentDownloaderFactory(HttpClientHandler delegatingHandler, AppConfiguration configuration)
        {
            this.delegatingHandler = delegatingHandler;
            this.configuration = configuration;
        }

        public ContentDownloader Create(Uri uri) => new ContentDownloader(configuration, delegatingHandler, uri);
    }
}
