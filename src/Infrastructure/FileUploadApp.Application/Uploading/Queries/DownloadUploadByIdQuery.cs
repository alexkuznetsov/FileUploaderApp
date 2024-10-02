using System;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

using MediatR;

namespace FileUploadApp.Application.Uploading.Queries;

public static class DownloadUploadById
{
    public record Query(Guid Id) : IRequest<Upload?>;

    public sealed class Handler(IStore<Guid, Upload, UploadResultRow> store) : IRequestHandler<Query, Upload?>
    {
        public Task<Upload?> Handle(Query request, CancellationToken cancellationToken = default)
            => store.ReceiveAsync(request.Id, cancellationToken);
    }

}
