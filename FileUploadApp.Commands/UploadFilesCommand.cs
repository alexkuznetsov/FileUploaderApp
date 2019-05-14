using FileUploadApp.Domain;
using MediatR;
using System.Collections.Generic;

namespace FileUploadApp.Commands
{
    public class UploadFilesCommand : IRequest<UploadResult>
    {
        public UploadFilesCommand(IEnumerable<Upload> uploadedFiles)
        {
            UploadedFiles = uploadedFiles;
        }

        public IEnumerable<Upload> UploadedFiles { get; }
    }
}
