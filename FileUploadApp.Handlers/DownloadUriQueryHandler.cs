using FileUploadApp.Domain.Dirty;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class DownloadUriQueryHandler : IRequestHandler<DownloadUriQuery, DownloadUriResponse>
    {
        private readonly IContentDownloaderFactory<DownloadUriResponse> contentDownloaderFactory;

        public DownloadUriQueryHandler(IContentDownloaderFactory<DownloadUriResponse> contentDownloaderFactory)
        {
            this.contentDownloaderFactory = contentDownloaderFactory;
        }

        public async Task<DownloadUriResponse> Handle(DownloadUriQuery request, CancellationToken cancellationToken)
        {
            var download = contentDownloaderFactory.Create(request.Uri);

            return await download.DownloadAsync(cancellationToken);
        }
    }
}
