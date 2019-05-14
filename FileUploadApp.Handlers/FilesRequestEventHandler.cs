using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class FilesRequestEventHandler : INotificationHandler<FileUploadEvent>
    {
        private readonly IContentTypeTestUtility contentTypeChecker;
        private readonly UploadsContext uploadedFilesContext;

        public FilesRequestEventHandler(IContentTypeTestUtility contentTypeChecker, UploadsContext uploadedFilesContext)
        {
            this.contentTypeChecker = contentTypeChecker;
            this.uploadedFilesContext = uploadedFilesContext;
        }

        public Task Handle(FileUploadEvent notification, CancellationToken cancellationToken)
        {
            foreach (var file in notification.FormFileDescriptors)
            {
                if (contentTypeChecker.IsAllowed(file.ContentType))
                {
                    uploadedFilesContext.Add(
                        number: file.Number,
                        name: file.Name,
                        contentType: file.ContentType,
                        streamAdapter: file.Stream);
                }
            }

            return Task.FromResult(0);
        }
    }
}
