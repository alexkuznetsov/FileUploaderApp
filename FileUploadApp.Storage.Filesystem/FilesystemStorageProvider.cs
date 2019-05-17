using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System.IO;

namespace FileUploadApp.Storage.Filesystem
{
    public class FilesystemStorageProvider : IStorageProvider<Upload, UploadResultRow>
    {
        private readonly StorageConfiguration configuration;
        private readonly ISerializer serializer;
        private readonly IDeserializer deserializer;

        public FilesystemStorageProvider(StorageConfiguration configuration, ISerializer serializer
            , IDeserializer deserializer)
        {
            this.configuration = configuration;
            this.serializer = serializer;
            this.deserializer = deserializer;
        }

        public IStorage<Upload, UploadResultRow> GetStorage()
        {
            if (!Directory.Exists(configuration.BasePath))
            {
                Directory.CreateDirectory(configuration.BasePath);
            }

            return new FileSystemStore(configuration.BasePath, serializer, deserializer);
        }
    }
}
