using FileUploadApp.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class UploadRequestEventHandler : INotificationHandler<UploadRequestEvent>
    {
        private readonly IMediator mediator;

        public UploadRequestEventHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task Handle(UploadRequestEvent notification, CancellationToken cancellationToken)
        {
            var model = notification.UploadRequest;
            var isModelOk = model != null;
            var isFilesNotEmpty = isModelOk && model.Files != null && model.Files.Length > 0;
            var isLinksNotEmpty = isModelOk && model.Links != null && model.Links.Length > 0;

            if (isFilesNotEmpty)
            {
                return mediator.Publish(new ProcessImageBase64Event(model.Files), cancellationToken);
            }
            else if (isLinksNotEmpty)
            {
                return mediator.Publish(new ProcessImageUriEvent(model.Links), cancellationToken);
            }

            else
                throw new InvalidOperationException("Empty request for processing supplied");
        }
    }
}
