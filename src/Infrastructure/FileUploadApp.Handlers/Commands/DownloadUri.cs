using FileUploadApp.Domain;
using FileUploadApp.Domain.Raw;
using FileUploadApp.Interfaces;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Features.Commands;

public class DownloadUri
{
    public class Command : IRequest<Upload>
    {
        public Command(uint number, Uri uri)
        {
            Number = number;
            Uri = uri;
        }

        public uint Number { get; }
        public Uri Uri { get; }
    }


    public class Handler : IRequestHandler<Command, Upload>
    {
        private readonly IContentDownloader<DownloadUriResponse> _contentDownloader;

        public Handler(IContentDownloader<DownloadUriResponse> contentDownloader)
        {
            this._contentDownloader = contentDownloader;
        }

        public async Task<Upload> Handle(Command request, CancellationToken cancellationToken)
        {
            var downloaded = await _contentDownloader.DownloadAsync(request.Uri,
                    cancellationToken).ConfigureAwait(false);

            return new Upload
            (
                id: Guid.NewGuid(),
                previewId: Guid.NewGuid(),
                num: request.Number,
                name: Path.GetFileName(request.Uri.LocalPath),
                contentType: downloaded.ContentType,
                streamAdapter: downloaded.Stream
            );
        }
    }

}
