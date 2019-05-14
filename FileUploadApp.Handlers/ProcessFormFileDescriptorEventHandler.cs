using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class ProcessFormFileDescriptorEventHandler : INotificationHandler<ProcessFileDescriptorEvent>
    {
        private readonly IContentTypeTestUtility contentTypeTestUtility;
        private readonly UploadsContext uploadedFilesContext;
        private static readonly int MinBytesForDetection = 4;

        public ProcessFormFileDescriptorEventHandler(IContentTypeTestUtility contentTypeTestUtility,
                                         UploadsContext uploadedFilesContext)
        {
            this.contentTypeTestUtility = contentTypeTestUtility;
            this.uploadedFilesContext = uploadedFilesContext;
        }

        public async Task Handle(ProcessFileDescriptorEvent request, CancellationToken cancellationToken)
        {
            var files = request.Files
                .Select(f => HandleCore(f, cancellationToken))
                .ToArray();

            await Task.WhenAll(files);
        }

        private async Task HandleCore(FileDescriptor f, CancellationToken cancellationToken)
        {
            string contentType = f.ContentType;

            if (string.IsNullOrEmpty(f.ContentType))
            {
                var bytes = await f.Stream.AsBytesSlice(MinBytesForDetection, cancellationToken);
                contentType = contentTypeTestUtility.DetectContentType(bytes.Span);
            }

            if (contentTypeTestUtility.IsAllowed(contentType))
            {
                uploadedFilesContext.Add(
                    number: f.Number,
                    name: f.Name,
                    contentType: contentType,
                    streamAdapter: f.Stream
                );
            }
        }
    }
}
