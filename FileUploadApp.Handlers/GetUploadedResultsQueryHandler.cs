using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class GetUploadedResultsQueryHandler : IRequestHandler<GetUploadedResultsQuery, UploadResult>
    {
        private readonly IStore<Upload, UploadResultRow> store;

        public GetUploadedResultsQueryHandler(IStore<Upload, UploadResultRow> store)
        {
            this.store = store;
        }

        public async Task<UploadResult> Handle(GetUploadedResultsQuery request, CancellationToken cancellationToken)
        {
            var tasks = request.Ids.Select(x => ReceiveAsync(x, cancellationToken))
                .ToArray();

            var results = await Task.WhenAll(tasks);

            return new UploadResult(results);
        }

        private async Task<UploadResultRow> ReceiveAsync(Tuple<Guid, Guid> fileIdPreviewId, CancellationToken cancellationToken)
        {
            var file = await store.ReceiveAsync(fileIdPreviewId.Item1, cancellationToken);
            var row = new UploadResultRow(file.Id, file.Number, file.Name, file.ContentType);

            if (file.IsImage())
            {
                var preview = await store.ReceiveAsync(fileIdPreviewId.Item2, cancellationToken);

                row.Preview = new FileEntity(preview.Id, preview.Number, preview.Name, preview.ContentType);
            }

            return row;
        }
    }
}
