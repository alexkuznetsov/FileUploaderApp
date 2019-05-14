using FileUploadApp.Interfaces;
using System.IO;

namespace FileUploadApp.Storage.Filesystem
{
    public class FilesystemStorageProvider : IStorageProvider
    {
        private readonly StorageConfiguration configuration;
        private readonly SpecHandler specHandler;

        public FilesystemStorageProvider(StorageConfiguration configuration, SpecHandler specHandler)
        {
            this.configuration = configuration;
            this.specHandler = specHandler;
        }

        public IStorage GetStorage()
        {
            if (!Directory.Exists(configuration.BasePath))
            {
                Directory.CreateDirectory(configuration.BasePath);
            }

            return new FileSystemStore(configuration.BasePath, specHandler);
        }
    }
}
