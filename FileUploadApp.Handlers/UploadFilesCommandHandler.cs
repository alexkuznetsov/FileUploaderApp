using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.Imaging;
using FileUploadApp.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class UploadFilesCommandHandler : INotificationHandler<UploadFilesEvent>
    {
        private readonly IStorage<Upload, UploadResultRow> storage;
        private readonly AppConfiguration appConfiguration;

        public UploadFilesCommandHandler(IStorageProvider<Upload, UploadResultRow> storageProvider, AppConfiguration appConfiguration)
        {
            storage = storageProvider.GetStorage();
            this.appConfiguration = appConfiguration;
        }

        public async Task Handle(UploadFilesEvent notification, CancellationToken cancellationToken)
        {
            var tasks = notification.UploadedFiles.Select(x => SaveFileAsync(x));

            await Task.WhenAll(tasks)
                .ContinueWith(x => new UploadResult(x.Result));
        }

        private async Task<UploadResultRow> SaveFileAsync(Upload file)
        {
            var result = await storage.StoreAsync(file).ConfigureAwait(false);

            if (file.IsImage())
            {
                var preview = await CreatePreviewAsync(file);

                result.Preview = await storage.StoreAsync(preview).ConfigureAwait(false);
            }

            return result;
        }

        private async Task<Upload> CreatePreviewAsync(Upload origin, CancellationToken cancellationToken = default)
        {
            using (var image = await ImageHelper
                    .FromUploadAsync(origin, cancellationToken)
                    .ConfigureAwait(false))
            {
                return image.Resize(appConfiguration.PreviewSize);
            }
        }
    }
}
