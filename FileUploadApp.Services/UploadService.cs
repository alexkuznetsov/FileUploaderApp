using FileUploadApp.Domain;
using FileUploadApp.Imaging;
using FileUploadApp.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApp.Services
{
    public class UploadService : IUploadService
    {
        private readonly IStorage storage;

        public UploadService(IStorageProvider storageProvider)
        {
            storage = storageProvider.GetStorage();
        }

        public ICollection<UploadedFile> UploadedFiles { get; set; } = new List<UploadedFile>();

        public Task<UploadResult> UploadAsync()
        {
            var tasks = UploadedFiles.Select(x => SaveFileAsync(x));

            return Task.WhenAll(tasks)
                .ContinueWith(x => Task.FromResult(new UploadResult(x.Result)))
                .Unwrap();
        }

        private async Task<UploadResultRow> SaveFileAsync(UploadedFile file)
        {
            var preview = await CreatePreviewAsync(file);
            var savedOriginal = await storage.StoreAsync(file).ConfigureAwait(false);
            var savedPriview = await storage.StoreAsync(preview).ConfigureAwait(false);

            savedOriginal.Preview = savedPriview;

            return savedOriginal;
        }

        private async Task<UploadedFile> CreatePreviewAsync(UploadedFile origin)
        {
            using (var image = await ImageHelper.CreateImageAsync(origin)
                .ConfigureAwait(false))

            {
                return ImageHelper.Resize(origin, image, 100, 100, MimeConstants.JpgMime);
            }
        }
    }
}
