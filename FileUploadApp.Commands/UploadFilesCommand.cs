using FileUploadApp.Domain;
using MediatR;
using System.Collections.Generic;

namespace FileUploadApp.Commands
{
    public class UploadFilesCommand : IRequest<UploadResult>
    {
        public UploadFilesCommand(IEnumerable<UploadedFile> uploadedFiles)
        {
            UploadedFiles = uploadedFiles;
        }

        public IEnumerable<UploadedFile> UploadedFiles { get; }
    }
}
