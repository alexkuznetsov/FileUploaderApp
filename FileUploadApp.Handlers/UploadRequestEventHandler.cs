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
            var isModelOk = notification != null;
            var isFilesNotEmpty = isModelOk && notification.Files != null && notification.Files.Length > 0;
            var isLinksNotEmpty = isModelOk && notification.Links != null && notification.Links.Length > 0;

            if (isFilesNotEmpty)
            {
                return mediator.Publish(new ProcessFileDescriptorEvent(notification.Files), cancellationToken);
            }
            else if (isLinksNotEmpty)
            {
                return mediator.Publish(new ProcessImageUriEvent(notification.Links), cancellationToken);
            }

            else
                throw new InvalidOperationException("Empty request for processing supplied");
        }
    }
}
