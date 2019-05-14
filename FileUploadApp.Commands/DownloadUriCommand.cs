using MediatR;
using System;

namespace FileUploadApp.Commands
{
    public class DownloadUriResponse
    {
        public DownloadUriResponse(Uri uri, byte[] bytea)
        {
            Uri = uri;
            Bytea = bytea;
        }

        public Uri Uri { get; }
        public byte[] Bytea { get; }
    }

    public class DownloadUriCommand : IRequest<DownloadUriResponse>
    {
        public DownloadUriCommand(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }
}
