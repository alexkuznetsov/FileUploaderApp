using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Features.Queries;

public class GetUploadedResults
{
    public class Query : IRequest<UploadResult>
    {
        public Query(IEnumerable<Upload> uploads)
        {
            Ids = uploads
                .Select(x => new Tuple<Guid, Guid>(x.Id, x.PreviewId))
                .ToArray();
        }

        public Tuple<Guid, Guid>[] Ids { get; }
    }

    public class Handler : IRequestHandler<Query, UploadResult>
    {
        private readonly IStore<Guid, Upload, UploadResultRow> store;

        public Handler(IStore<Guid, Upload, UploadResultRow> store)
        {
            this.store = store;
        }

        public async Task<UploadResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var tasks = request.Ids.Select(x => ReceiveAsync(x, cancellationToken)).ToArray();
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            return new UploadResult(results);
        }

        private async Task<UploadResultRow> ReceiveAsync(Tuple<Guid, Guid> fileIdPreviewId, CancellationToken cancellationToken)
        {
            var (fileId, previewId) = fileIdPreviewId;
            var file = await store.ReceiveAsync(fileId, cancellationToken).ConfigureAwait(false);
            var row = new UploadResultRow(file.Id, file.Number, file.Name, file.ContentType);

            if (!file.IsImage()) return row;

            var preview = await store.ReceiveAsync(previewId, cancellationToken).ConfigureAwait(false);

            row.Preview = new FileEntity(preview.Id, preview.Number, preview.Name, preview.ContentType);

            return row;
        }
    }

}
