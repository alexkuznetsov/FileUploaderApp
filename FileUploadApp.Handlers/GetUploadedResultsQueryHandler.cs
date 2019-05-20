using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using FileUploadApp.Storage;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class GetUploadedResultsQueryHandler : IRequestHandler<GetUploadedResultsQuery, UploadResult>
    {
        private readonly IStorage<Upload, UploadResultRow> storage;

        public GetUploadedResultsQueryHandler(IStorageProvider<Upload, UploadResultRow> storageProvider)
        {
            storage = storageProvider.GetStorage();
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
            var file = await storage.ReceiveAsync(fileIdPreviewId.Item1, cancellationToken);
            var row = new UploadResultRow(file.Id, file.Number, file.Name, file.ContentType);

            if (file.IsImage())
            {
                var preview = await storage.ReceiveAsync(fileIdPreviewId.Item2, cancellationToken);

                row.Preview = new FileEntity(preview.Id, preview.Number, preview.Name, preview.ContentType);
            }

            return row;
        }
    }
}
