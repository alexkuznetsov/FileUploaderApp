using System;

namespace FileUploadApp.Domain.Dirty
{
    public class DownloadUriResponse
    {
        public DownloadUriResponse(Uri uri, string contentType, byte[] bytea)
        {
            Uri = uri;
            Bytea = bytea;
            ContentType = contentType;
        }

        public string ContentType { get; }

        public Uri Uri { get; }

        public byte[] Bytea { get; }
    }
}