using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.Imaging;
using FileUploadApp.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

            var preview = ImageHelper.Resize(appConfiguration.PreviewSize, file);

            result.Preview = await store.StoreAsync(preview).ConfigureAwait(false);

            return result;
        }
    }
}
