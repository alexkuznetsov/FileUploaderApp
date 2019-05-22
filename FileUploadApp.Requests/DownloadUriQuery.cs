using FileUploadApp.Domain;
using FileUploadApp.Domain.Dirty;
using MediatR;
using System;

namespace FileUploadApp.Requests
{
    public class DownloadUriQuery : IRequest<Upload>
    {
        public DownloadUriQuery(uint number, Uri uri)
        {
            Number = number;
            Uri = uri;
        }

        public uint Number { get; }
        public Uri Uri { get; }
    }
}
