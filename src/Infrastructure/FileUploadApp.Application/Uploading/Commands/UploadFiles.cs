using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Application.Common;
using FileUploadApp.Application.Common.Messaging;
using FileUploadApp.Domain;
using FileUploadApp.Imaging;
using FileUploadApp.Interfaces;

using Microsoft.Extensions.Logging;

namespace FileUploadApp.Application.Uploading.Commands;

public static class UploadFiles
{
    public record Command(IEnumerable<Upload> UploadedFiles) : ICommand<Result>;

    public record Result : ResultBase<Result>;

    public class Handler(IStore<Guid, Upload, UploadResultRow> store
            , AppConfiguration appConfiguration
            , ILogger<Handler> logger) : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var tasks = command.UploadedFiles
                .Select(async e => await SaveFileAsync(e, cancellationToken))
                .ToArray();

            await Task.WhenAll(tasks);

            return Result.Ok();
        }

        private async Task<UploadResultRow> SaveFileAsync(Upload uploadModel, CancellationToken cancellationToken)
        {
            logger.LogInformation("Saving the file {fileName}. Content type: {fileContentType}"
                , uploadModel.Name, uploadModel.ContentType);

            var result = await store.StoreAsync(uploadModel, cancellationToken);

            if (!uploadModel.IsImage())
            {
                uploadModel.Stream.Dispose();

                return result;
            }

            logger.LogInformation("Saving preview for the file {fileName}. Content type: {fileContentType}",
                uploadModel.Name, uploadModel.ContentType);

            using var previewData = ImageHelper.Resize(appConfiguration.PreviewSize
                    , uploadModel.Stream
                    , appConfiguration.PreviewContentType);

            var preview = new Upload(
                  id: uploadModel.PreviewId.GetValueOrDefault()
                , previewId: Guid.Empty
                , num: uploadModel.Number
                , name: $"{Upload.PreviewPrefix}{uploadModel.Name}"
                , contentType: appConfiguration.PreviewContentType
                , data: previewData);

            result.Preview = await store.StoreAsync(preview, cancellationToken);

            return result;
        }
    }

}
