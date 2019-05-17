using FileUploadApp.Requests;
using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.StreamAdapters;
using MediatR;
using System;
using System.IO;
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
                         .Select(x => DownloadDataAsync(x.Item1, x.Item2, cancellationToken))
                         .ToArray();

            var result = await Task.WhenAll(tasks).ConfigureAwait(false);
            var @event = new ProcessFileDescriptorEvent(result);

            await mediator.Publish(@event, cancellationToken).ConfigureAwait(false);
        }

        private async Task<FileDescriptor> DownloadDataAsync(uint number, Uri uri, CancellationToken cancellationToken)
        {
            var data = await mediator.Send(new DownloadUriQuery(uri), cancellationToken)
                .ConfigureAwait(false);

            return new FileDescriptor(
                num: number,
                name: Path.GetFileName(uri.LocalPath),
                contentType: data.ContentType,
                stream: new ByteaStreamAdapter(data.Bytea)
             );
        }
    }
}
