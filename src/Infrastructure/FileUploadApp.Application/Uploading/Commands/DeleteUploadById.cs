using System;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Application.Common;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

using MediatR;

namespace FileUploadApp.Application.Uploading.Commands;

public static class DeleteUploadById
{
    public record Command(Guid Id) : IRequest<Result>;

    public record Result : ResultBase<Result>;

    public sealed class Handler(IStore<Guid, Upload, UploadResultRow> store)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var status = await store.DeleteAsync(request.Id, cancellationToken)
                .ConfigureAwait(false);

            return status ? Result.Ok() : Result.NotFound();
        }
    }
}
