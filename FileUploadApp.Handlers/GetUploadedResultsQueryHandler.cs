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
            var file = await storage.ReceiveAsync(fileIdPreviewId.Item1.ToString()
                , cancellationToken);
            var preview = await storage.ReceiveAsync(fileIdPreviewId.Item2.ToString()
                , cancellationToken);

            var row = new UploadResultRow
            {
                Preview = new UploadResultRowBase()
            };

            Fill(row, file);
            Fill(row.Preview, preview);

            return row;
        }

        private void Fill<TModel>(TModel model, Upload file)
            where TModel : UploadResultRowBase
        {
            model.ContentType = file.ContentType;
            model.Height = file.Height;
            model.Id = file.Id.ToString();
            model.Name = file.Name;
            model.Number = file.Number;
            model.Width = file.Width;
        }
    }
}
