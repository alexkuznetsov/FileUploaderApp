using FileUploadApp.Domain;
using MediatR;

namespace FileUploadApp.Requests
{
    public class DownloadUploadByIdQuery : IRequest<Upload>
    {
        public DownloadUploadByIdQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
