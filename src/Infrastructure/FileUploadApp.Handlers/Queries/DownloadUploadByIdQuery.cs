using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Features.Queries
{
    public class DownloadUploadById
    {
        public class Query : IRequest<Upload>
        {
            public Query(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; }
        }

        public class Handler : IRequestHandler<Query, Upload>
        {
            private readonly IStore<Guid, Upload, UploadResultRow> store;

            public Handler(IStore<Guid, Upload, UploadResultRow> store)
            {
                this.store = store;
            }

            public async Task<Upload> Handle(Query request, CancellationToken cancellationToken = default)
                => await store.ReceiveAsync(request.Id, cancellationToken).ConfigureAwait(false);
        }

    }
}
