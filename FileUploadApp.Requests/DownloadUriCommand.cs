using FileUploadApp.Domain.Dirty;
using MediatR;
using System;

namespace FileUploadApp.Requests
{
    public class DownloadUriQuery : IRequest<DownloadUriResponse>
    {
        public DownloadUriQuery(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }
}
