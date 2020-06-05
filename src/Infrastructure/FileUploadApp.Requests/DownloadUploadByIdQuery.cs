using FileUploadApp.Domain;
using MediatR;
using System;

namespace FileUploadApp.Requests
{
    public class DownloadUploadByIdQuery : IRequest<Upload>
    {
        public DownloadUploadByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
