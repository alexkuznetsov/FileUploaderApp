using FileUploadApp.Domain;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using FileUploadApp.StreamAdapters;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class DownloadUriQueryHandler : IRequestHandler<DownloadUriQuery, Upload>
    {
        private readonly IContentDownloaderFactory<DownloadUriResponse> contentDownloaderFactory;

        public DownloadUriQueryHandler(IContentDownloaderFactory<DownloadUriResponse> contentDownloaderFactory)
        {
            this.contentDownloaderFactory = contentDownloaderFactory;
        }

        public async Task<Upload> Handle(DownloadUriQuery request, CancellationToken cancellationToken)
        {
            var download = contentDownloaderFactory.Create(request.Uri);
            var downloaded = await download.DownloadAsync(cancellationToken)
                .ConfigureAwait(false);

            return new Upload
            (
                id: Guid.NewGuid(),
                previewId: Guid.NewGuid(),
                num: request.Number,
                name: Path.GetFileName(request.Uri.LocalPath),
                contentType: downloaded.ContentType,
                streamAdapter: new ByteaStreamAdapter(downloaded.Bytes)
            );
        }
    }
}
