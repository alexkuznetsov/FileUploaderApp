using FileUploadApp.Domain.Dirty;
using FileUploadApp.Requests;
using FileUploadApp.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class DownloadUriQueryHandler : IRequestHandler<DownloadUriQuery, DownloadUriResponse>
    {
        private readonly ContentDownloaderFactory contentDownloaderFactory;

        public DownloadUriQueryHandler(ContentDownloaderFactory contentDownloaderFactory)
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
