﻿using System;
using System.IO;

namespace FileUploadApp.Domain.Raw;

public class DownloadUriResponse
{
    public DownloadUriResponse(Uri uri, string contentType, Stream stream)
    {
        Uri = uri;
        ContentType = contentType;
        Stream = stream;
    }

    public string ContentType { get; }
    public Stream Stream { get; }
    public Uri Uri { get; }
}