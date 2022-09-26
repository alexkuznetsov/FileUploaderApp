﻿using FileUploadApp.Domain;
using FileUploadApp.Domain.Raw;
using FileUploadApp.Interfaces;
using System;
using System.Net.Http;

namespace FileUploadApp.Features.Services;

public class ContentDownloaderFactory : IContentDownloaderFactory<DownloadUriResponse>
{
    private readonly HttpClientHandler delegatingHandler;
    private readonly AppConfiguration configuration;

    public ContentDownloaderFactory(HttpClientHandler delegatingHandler, AppConfiguration configuration)
    {
        this.delegatingHandler = delegatingHandler;
        this.configuration = configuration;
    }

    public IContentDownloader<DownloadUriResponse> Create(Uri uri) => new ContentDownloader(configuration, delegatingHandler, uri);
}
