using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Application.Common;
using FileUploadApp.Domain;
using FileUploadApp.Domain.Raw;
using FileUploadApp.Interfaces;

using MediatR;

namespace FileUploadApp.Application.Uploading.Commands;

public static class DownloadUri
{
    public record Command(uint Number, Uri Uri) : IRequest<Result>;

    public record Result : ResultBase<Result, Upload>;

    public sealed class Handler(IContentDownloader<DownloadUriResponse> contentDownloader) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var downloaded = await contentDownloader.DownloadAsync(request.Uri,
                    cancellationToken).ConfigureAwait(false);

            return Result.Ok(new Upload
            (
                id: Guid.NewGuid(),
                previewId: Guid.NewGuid(),
                num: request.Number,
                name: Path.GetFileName(request.Uri.LocalPath),
                contentType: downloaded.ContentType,
                data: downloaded.Stream
            ));
        }
    }

}
