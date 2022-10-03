using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Features.Commands
{
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
            private readonly IStore<Guid, Upload, UploadResultRow> store;
            private readonly AppConfiguration appConfiguration;
            private readonly ILogger<Handler> logger;

            public Handler(IStore<Guid, Upload, UploadResultRow> store
                , AppConfiguration appConfiguration
                , ILogger<Handler> logger)
            {
                this.store = store;
                this.appConfiguration = appConfiguration;
                this.logger = logger;
            }
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var status = await store.DeleteAsync(request.Id, cancellationToken)
                    .ConfigureAwait(false);

                return status ? Result.Ok() : Result.NotFound();
            }
        }
    }
}
