using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.Imaging;
using FileUploadApp.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileUploadApp.StreamAdapters;

namespace FileUploadApp.Handlers
{
    public class UploadFilesCommandHandler : INotificationHandler<UploadFilesEvent>
    {
        private readonly IStore<Guid, Upload, UploadResultRow> store;
        private readonly AppConfiguration appConfiguration;
        private readonly ILogger<UploadFilesCommandHandler> logger;

        public UploadFilesCommandHandler(IStore<Guid, Upload, UploadResultRow> store, AppConfiguration appConfiguration, ILogger<UploadFilesCommandHandler> logger)
        {
            this.store = store;
            this.appConfiguration = appConfiguration;
            this.logger = logger;
        }

        public async Task Handle(UploadFilesEvent notification, CancellationToken cancellationToken)
        {
            var tasks = notification.UploadedFiles.Select(SaveFileAsync).ToArray();

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task<UploadResultRow> SaveFileAsync(Upload file)
        {
            logger.LogInformation($"saving file {file.Name}. Content type: {file.ContentType}");
            var result = await store.StoreAsync(file).ConfigureAwait(false);

            if (!file.IsImage()) return result;
            
            logger.LogInformation($"saving preview for file {file.Name}. Content type: {file.ContentType}");

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
