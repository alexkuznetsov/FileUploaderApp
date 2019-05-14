using FileUploadApp.Domain.Dirty;
using MediatR;
using System;

namespace FileUploadApp.Commands
{
    public class DownloadUriCommand : IRequest<DownloadUriResponse>
    {
        public DownloadUriCommand(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }
}
