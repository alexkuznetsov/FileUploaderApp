using System;

namespace FileUploadApp.Domain.Raw;

public class DownloadUriResponse
{
    public DownloadUriResponse(Uri uri, string contentType, byte[] bytes)
    {
        Uri = uri;
        Bytes = bytes;
        ContentType = contentType;
    }

    public string ContentType { get; }

    public Uri Uri { get; }

    public byte[] Bytes { get; }
}