using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class DownloadUploadByIdQueryHandler : IRequestHandler<DownloadUploadByIdQuery, Upload>
    {
        private readonly IStorage<Upload, UploadResultRow> storage;

        public DownloadUploadByIdQueryHandler(IStorageProvider<Upload, UploadResultRow> storageProvider)
        {
            storage = storageProvider.GetStorage();
        }

        public Task<Upload> Handle(DownloadUploadByIdQuery request, CancellationToken cancellationToken)
            => storage.ReceiveAsync(request.Id, cancellationToken);
    }
}
