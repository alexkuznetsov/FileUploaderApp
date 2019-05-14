using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class FilesRequestEventHandler : INotificationHandler<FilesRequestEvent>
    {
        private readonly IContentTypeTestUtility contentTypeChecker;
        private readonly UploadedFilesContext uploadedFilesContext;

        public FilesRequestEventHandler(IContentTypeTestUtility contentTypeChecker, UploadedFilesContext uploadedFilesContext)
        {
            this.contentTypeChecker = contentTypeChecker;
            this.uploadedFilesContext = uploadedFilesContext;
        }

        public Task Handle(FilesRequestEvent notification, CancellationToken cancellationToken)
        {
            foreach (var file in notification.FormFileDescriptors)
            {
                if (contentTypeChecker.IsAllowed(file.ContentType))
                {
                    uploadedFilesContext.Add(
                        number: file.Number,
                        name: file.Name,
                        contentType: file.ContentType,
                        streamWrapper: file.Stream);
                }
            }

            return Task.FromResult(0);
        }
    }
}
