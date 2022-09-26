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

            private async Task<UploadResultRow> SaveFileAsync(Upload file)
            {
                logger.LogInformation("Saving file {fileName}. Content type: {fileContentType}"
                    , file.Name, file.ContentType);

                var result = await store.StoreAsync(file).ConfigureAwait(false);

                if (!file.IsImage()) return result;

                logger.LogInformation("Saving preview for file {fileName}. Content type: {fileContentType}",
                    file.Name, file.ContentType);

                using (var stream = new MemoryStream())
                {
                    await file.Stream.CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    var previewBytes = ImageHelper.Resize(appConfiguration.PreviewSize, stream);
                    var preview = new Upload(
                        id: file.PreviewId
                        , previewId: Guid.Empty
                        , num: file.Number
                        , name: $"{Upload.PreviewPrefix}{file.Name}"
                        , contentType: file.ContentType
                        , streamAdapter: new ByteaStreamAdapter(previewBytes));

                    result.Preview = await store.StoreAsync(preview).ConfigureAwait(false);
                }

                return result;
            }
        }

    }
}
