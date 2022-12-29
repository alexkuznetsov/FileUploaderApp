using System;

namespace FileUploadApp.Domain.Raw;

public class DownloadUriResponse
{
    public DownloadUriResponse(Uri uri, string contentType, StreamAdapter stream)
    {
        Uri = uri;
        ContentType = contentType;
        Stream = stream;
    }

    public string ContentType { get; }
    public StreamAdapter Stream { get; }
    public Uri Uri { get; }
}