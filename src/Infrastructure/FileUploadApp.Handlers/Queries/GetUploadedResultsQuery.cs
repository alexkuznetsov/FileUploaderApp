using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

using MediatR;

namespace FileUploadApp.Features.Queries;

public class GetUploadedResults
{
    public class Query : IRequest<UploadResult>
    {
        public Query(IEnumerable<Upload> uploads)
        {
            Ids = uploads
                .Select(x => new Tuple<Guid, Guid>(x.Id, x.PreviewId.GetValueOrDefault()))
                .ToArray();
        }

        public Tuple<Guid, Guid>[] Ids { get; }
    }

    public class Handler : IRequestHandler<Query, UploadResult>
    {
        private readonly IStore<Guid, Upload, UploadResultRow> _store;

        public Handler(IStore<Guid, Upload, UploadResultRow> store)
        {
            this._store = store;
        }

        public async Task<UploadResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var tasks = request.Ids.Select(x => ReceiveAsync(x, cancellationToken)).ToArray();
            var results = await Task.WhenAll(tasks) ?? [];

            return new UploadResult(results);
        }

        private async Task<UploadResultRow?> ReceiveAsync(Tuple<Guid, Guid> fileIdPreviewId, CancellationToken cancellationToken)
        {
            var (fileId, previewId) = fileIdPreviewId;
            var file = await _store.ReceiveAsync(fileId, cancellationToken).ConfigureAwait(false);

            if (file == null)
            {
                return null;
            }

            var row = new UploadResultRow(file.Id, file.Number, file.Name, file.ContentType);

            if (!file.IsImage()) return row;

            var preview = await _store.ReceiveAsync(previewId, cancellationToken).ConfigureAwait(false);

            if (preview != null)
            {
                row.Preview = new FileEntity(preview.Id, preview.Number, preview.Name, preview.ContentType);
            }

            return row;
        }
    }

}
