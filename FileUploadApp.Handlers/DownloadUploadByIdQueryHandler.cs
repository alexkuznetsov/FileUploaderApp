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
        private readonly IStore<Upload, UploadResultRow> store;

        public DownloadUploadByIdQueryHandler(IStore<Upload, UploadResultRow> store)
        {
            this.store = store;
        }

        public async Task<Upload> Handle(DownloadUploadByIdQuery request, CancellationToken cancellationToken = default)
            => await store.ReceiveAsync(request.Id, cancellationToken).ConfigureAwait(false);
    }
}
