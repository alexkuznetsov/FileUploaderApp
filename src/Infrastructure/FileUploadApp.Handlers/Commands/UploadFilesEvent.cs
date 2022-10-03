using FileUploadApp.Domain;
using FileUploadApp.Imaging;
using FileUploadApp.Interfaces;
using FileUploadApp.StreamAdapters;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Features.Commands
{
    public class UploadFiles

    {
        public class Event : GenericEvent
        {
            public Event(IEnumerable<Upload> uploadedFiles)
            {
                UploadedFiles = uploadedFiles;
            }

            public IEnumerable<Upload> UploadedFiles { get; }
        }

        public class Handler : INotificationHandler<Event>
        {
            private readonly IStore<Guid, Upload, UploadResultRow> store;
            private readonly AppConfiguration appConfiguration;
            private readonly ILogger<Handler> logger;

            public Handler(IStore<Guid, Upload, UploadResultRow> store
                , AppConfiguration appConfiguration
                , ILogger<Handler> logger)
            {
                this.store = store;
                this.appConfiguration = appConfiguration;
                this.logger = logger;
            }

            public async Task Handle(Event notification, CancellationToken cancellationToken)
            {
                var tasks = notification.UploadedFiles.Select(SaveFileAsync).ToArray();

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }

            private async Task<UploadResultRow> SaveFileAsync(Upload uploadModel)
            {
                logger.LogInformation("Saving file {fileName}. Content type: {fileContentType}"
                    , uploadModel.Name, uploadModel.ContentType);

                var result = await store.StoreAsync(uploadModel).ConfigureAwait(false);

                if (!uploadModel.IsImage())
                {
                    if (uploadModel.Stream is CommonStreamStreamAdapter ci)
                    {
                        ci.Stream.Dispose();
                    }
                    else if (uploadModel.Stream is FormFileStreamAdapter fi)
                    {
                        fi.Stream.Dispose();
                    }

                    return result;
                }

                logger.LogInformation("Saving preview for file {fileName}. Content type: {fileContentType}",
                    uploadModel.Name, uploadModel.ContentType);

                using var previewBytes = ImageHelper.Resize(appConfiguration.PreviewSize
                        , uploadModel.Stream.Stream
                        , appConfiguration.PreviewContentType);

                var preview = new Upload(
                    id: uploadModel.PreviewId
                    , previewId: Guid.Empty
                    , num: uploadModel.Number
                    , name: $"{Upload.PreviewPrefix}{uploadModel.Name}"
                    , contentType: appConfiguration.PreviewContentType
                    , streamAdapter: new CommonStreamStreamAdapter(previewBytes));

                result.Preview = await store.StoreAsync(preview).ConfigureAwait(false);

                return result;
            }
        }

    }
}
