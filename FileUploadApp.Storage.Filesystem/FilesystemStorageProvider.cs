using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System;
using System.IO;

namespace FileUploadApp.Storage.Filesystem
{
    public class FilesystemStorageProvider : IStorageProvider<Upload, UploadResultRow>
    {
        private readonly StorageConfiguration configuration;
        private readonly IStoreBackend<Guid, Metadata> metadataStorageBackend;
        private readonly IStoreBackend<Guid, Upload> uploadsStorageBackend;
        private readonly IFileStreamProvider<Guid, StreamAdapter> fileStreamProvider;

        public FilesystemStorageProvider(StorageConfiguration configuration
            , IStoreBackend<Guid, Metadata> metadataStorageBackend
            , IStoreBackend<Guid, Upload> uploadsStorageBackend
            , IFileStreamProvider<Guid, StreamAdapter> fileStreamProvider)
        {
            this.configuration = configuration;
            this.metadataStorageBackend = metadataStorageBackend;
            this.uploadsStorageBackend = uploadsStorageBackend;
            this.fileStreamProvider = fileStreamProvider;
        }

        public IStorage<Upload, UploadResultRow> GetStorage()
        {
            if (!Directory.Exists(configuration.BasePath))
            {
                Directory.CreateDirectory(configuration.BasePath);
            }

            //var pathExpander = new FilesystemPathExpander(configuration);
            //var storeBackend = new FilesystemStoreBackend(pathExpander);
            //var metaDataStoreBackend = new MetadataFSStoreBackend(pathExpander, serializer, deserializer);

            return new FileSystemStore(metadataStorageBackend, uploadsStorageBackend, fileStreamProvider);
        }
    }
}
