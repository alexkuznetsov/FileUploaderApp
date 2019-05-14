using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.Interfaces;
using FileUploadApp.StreamWrappers;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class AfterDownloadImageUriHandler : INotificationHandler<AfterDownloadImageUriEvent>
    {
        private readonly IContentTypeTestUtility contentTypeTestUtility;
        private readonly UploadedFilesContext uploadedFilesContext;

        private readonly int MaxBitsCountForBase64 = 4;

        public AfterDownloadImageUriHandler(IContentTypeTestUtility contentTypeTestUtility, UploadedFilesContext uploadedFilesContext)
        {
            this.contentTypeTestUtility = contentTypeTestUtility;
            this.uploadedFilesContext = uploadedFilesContext;
        }

        public Task Handle(AfterDownloadImageUriEvent notification, CancellationToken cancellationToken)
        {
            var byteArray = notification.Bytea.AsMemory();
            var base64 = Convert.ToBase64String(byteArray.Slice(0, MaxBitsCountForBase64).ToArray());
            var contentType = contentTypeTestUtility.DetectContentType(base64);

            if (contentTypeTestUtility.IsAllowed(contentType))
            {
                uploadedFilesContext.Add(
                    number: notification.Number,
                    name: Path.GetFileName(notification.Uri.LocalPath),
                    contentType: contentType,
                    streamWrapper: new ByteaStreamWrapper(byteArray));
            }

            return Task.FromResult(0);
        }
    }
}
