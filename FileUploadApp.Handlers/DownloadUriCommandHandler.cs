using FileUploadApp.Commands;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class DownloadUriCommandHandler : IRequestHandler<DownloadUriCommand, DownloadUriResponse>
    {
        private readonly ContentDownloaderFactory contentDownloaderFactory;

        public DownloadUriCommandHandler(ContentDownloaderFactory contentDownloaderFactory)
        {
            this.contentDownloaderFactory = contentDownloaderFactory;
        }

        public async Task<DownloadUriResponse> Handle(DownloadUriCommand request, CancellationToken cancellationToken)
        {
            var download = contentDownloaderFactory.Create(request.Uri);

            return await download.DownloadAsync(cancellationToken);
        }
    }
}
