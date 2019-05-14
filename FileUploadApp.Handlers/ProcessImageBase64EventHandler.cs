using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.Interfaces;
using FileUploadApp.StreamWrappers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class ProcessImageBase64EventHandler : INotificationHandler<ProcessImageBase64Event>
    {
        private readonly IContentTypeTestUtility contentTypeTestUtility;
        private readonly UploadedFilesContext uploadedFilesContext;

        public ProcessImageBase64EventHandler(IContentTypeTestUtility contentTypeTestUtility,
                                         UploadedFilesContext uploadedFilesContext)
        {
            this.contentTypeTestUtility = contentTypeTestUtility;
            this.uploadedFilesContext = uploadedFilesContext;
        }

        public Task Handle(ProcessImageBase64Event request, CancellationToken cancellationToken)
        {
            var files = request.Files;

            for (uint i = 0; i < request.Files.Length; i++)
            {
                var contentType = contentTypeTestUtility.DetectContentType(files[i].Base64);

                if (contentTypeTestUtility.IsAllowed(contentType))
                {
                    var bytea = Convert.FromBase64String(files[i].Base64);
                    var readOnlyMemory = new ReadOnlyMemory<byte>(bytea);

                    uploadedFilesContext.Add(
                        number: i,
                        name: files[i].Name,
                        contentType: contentType,
                        streamWrapper: new ByteaStreamWrapper(readOnlyMemory)
                    );
                }
            }

            return Task.FromResult(0);
        }
    }
}
