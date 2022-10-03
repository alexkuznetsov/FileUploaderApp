using System;

namespace FileUploadApp.Domain.Raw;

public class DownloadUriResponse
{
    [Obsolete("Use DownloadUriResponse(Uri uri, string contentType, StreamAdapter stream) instead")]
    public DownloadUriResponse(Uri uri, string contentType, byte[] bytes)
    {
        Uri = uri;
        Bytes = bytes;
        ContentType = contentType;
        StreamAdapter = null;
    }

    public DownloadUriResponse(Uri uri, string contentType, StreamAdapter stream)
    {
        Uri = uri;
        ContentType = contentType;
        Stream = stream;
    }

    public string ContentType { get; }
    public StreamAdapter Stream { get; }
    public Uri Uri { get; }

    [Obsolete("Use StreamAdapter instead")]
    public byte[] Bytes { get; } = Array.Empty<byte>();

    public StreamAdapter StreamAdapter { get; }
}