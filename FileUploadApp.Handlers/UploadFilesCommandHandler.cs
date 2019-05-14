using FileUploadApp.Commands;
using FileUploadApp.Domain;
using FileUploadApp.Imaging;
using FileUploadApp.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class UploadFilesCommandHandler : IRequestHandler<UploadFilesCommand, UploadResult>
    {
        private readonly IStorage<Upload, UploadResultRow> storage;

        public UploadFilesCommandHandler(IStorageProvider<Upload, UploadResultRow> storageProvider)
        {
            storage = storageProvider.GetStorage();
        }

        public Task<UploadResult> Handle(UploadFilesCommand request, CancellationToken cancellationToken)
        {
            var tasks = request.UploadedFiles.Select(x => SaveFileAsync(x));

            return Task.WhenAll(tasks)
                .ContinueWith(x => new UploadResult(x.Result));
        }

        private async Task<UploadResultRow> SaveFileAsync(Upload file)
        {
            var preview = await CreatePreviewAsync(file);
            var savedOriginal = await storage.StoreAsync(file).ConfigureAwait(false);
            var savedPriview = await storage.StoreAsync(preview).ConfigureAwait(false);

            savedOriginal.Preview = savedPriview;

            return savedOriginal;
        }

        private async Task<Upload> CreatePreviewAsync(Upload origin)
        {
            using (var image = await ImageHelper.CreateImageAsync(origin)
                .ConfigureAwait(false))

            {
                return ImageHelper.Resize(origin, image, 100, 100, origin.ContentType);
            }
        }
    }
}
