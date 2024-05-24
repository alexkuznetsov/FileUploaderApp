using System;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

using MediatR;

namespace FileUploadApp.Features.Commands;

public class DeleteUploadById
{
    public class Command : IRequest<Result>
    {
        public Command(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public class Result : ResultBase<Result>
    {

    }

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IStore<Guid, Upload, UploadResultRow> _store;

        public Handler(IStore<Guid, Upload, UploadResultRow> store)
        {
            this._store = store;
        }
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var status = await _store.DeleteAsync(request.Id, cancellationToken)
                .ConfigureAwait(false);

            return status ? Result.Ok() : Result.NotFound();
        }
    }
}
