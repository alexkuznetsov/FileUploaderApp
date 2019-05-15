using FileUploadApp.Domain;
using MediatR;

namespace FileUploadApp.Commands
{
    public class DownloadUploadByIdCommand : IRequest<Upload>
    {
        public DownloadUploadByIdCommand(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
