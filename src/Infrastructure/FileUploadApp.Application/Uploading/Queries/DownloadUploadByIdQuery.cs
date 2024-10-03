using System;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Application.Common.Messaging;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

namespace FileUploadApp.Application.Uploading.Queries;

public static class DownloadUploadById
{
    public record Query(Guid Id) : IMessage<Upload?>;

    public sealed class Handler(IStore<Guid, Upload, UploadResultRow> store)
        : IMessageHandler<Query, Upload?>
    {
        public Task<Upload?> Handle(Query request, CancellationToken cancellationToken = default)
            => store.ReceiveAsync(request.Id, cancellationToken);
    }

}
