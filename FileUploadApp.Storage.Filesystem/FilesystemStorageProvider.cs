using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System.IO;

namespace FileUploadApp.Storage.Filesystem
{
    public class FilesystemStorageProvider : IStorageProvider<Upload, UploadResultRow>
    {
        private readonly StorageConfiguration configuration;
        private readonly SpecHandler specHandler;

        public FilesystemStorageProvider(StorageConfiguration configuration, SpecHandler specHandler)
        {
            this.configuration = configuration;
            this.specHandler = specHandler;
        }

        public IStorage<Upload, UploadResultRow> GetStorage()
        {
            if (!Directory.Exists(configuration.BasePath))
            {
                Directory.CreateDirectory(configuration.BasePath);
            }

            return new FileSystemStore(configuration.BasePath, specHandler);
        }
    }
}
