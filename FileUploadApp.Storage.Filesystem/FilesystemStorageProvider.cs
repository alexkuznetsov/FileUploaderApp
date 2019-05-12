using FileUploadApp.Interfaces;
using System.IO;

namespace FileUploadApp.Storage.Filesystem
{
    public class FilesystemStorageProvider : IStorageProvider
    {
        private readonly string basePath;
        private readonly SpecHandler specHandler;

        public FilesystemStorageProvider(string basePath, SpecHandler specHandler)
        {
            this.basePath = basePath;
            this.specHandler = specHandler;
        }

        public IStorage GetStorage()
        {
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            return new FileSystemStore(basePath, specHandler);
        }
    }
}
