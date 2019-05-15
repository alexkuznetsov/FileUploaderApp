using FileUploadApp.Commands;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class DownloadUploadByIdCommandHandler : IRequestHandler<DownloadUploadByIdCommand, Upload>
    {
        private IStorage<Upload, UploadResultRow> storage;

        public DownloadUploadByIdCommandHandler(IStorageProvider<Upload, UploadResultRow> storageProvider)
        {
            storage = storageProvider.GetStorage();
        }

        public Task<Upload> Handle(DownloadUploadByIdCommand request, CancellationToken cancellationToken)
            => storage.ReceiveAsync(request.Id);
    }
}
