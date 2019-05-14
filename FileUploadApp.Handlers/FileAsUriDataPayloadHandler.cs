using FileUploadApp.Commands;
using FileUploadApp.Events;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class FileAsUriDataPayloadHandler : INotificationHandler<ProcessImageUriEvent>
    {
        private readonly IMediator mediator;

        public FileAsUriDataPayloadHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Handle(ProcessImageUriEvent notification, CancellationToken cancellationToken)
        {
            var tasks = notification.Links.AsUriEnumerable()
                         .Select(x => DownloadDataAsync(x, cancellationToken))
                         .ToList();

            await Task.WhenAll(tasks);
        }

        private async Task DownloadDataAsync(Uri uri, CancellationToken cancellationToken)
        {
            var data = await mediator.Send(new DownloadUriCommand(uri), cancellationToken)
                .ConfigureAwait(false);

            await mediator.Publish(new AfterDownloadImageUriEvent(data.Uri, data.Bytea))
                .ConfigureAwait(false);
        }
    }
}
