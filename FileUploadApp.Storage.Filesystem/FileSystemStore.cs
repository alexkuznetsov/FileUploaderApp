using FileUploadApp.Domain;
using System;

namespace FileUploadApp.Storage.Filesystem
{

    public class FileSystemStore : Store<Upload, UploadResultRow>
    {
        public FileSystemStore(IStoreBackend<Guid, Metadata> metadataRepository
            , IStoreBackend<Guid, Upload> storeBackend
            , IFileStreamProvider<Guid, StreamAdapter> fileStreamProvider) : base(metadataRepository, storeBackend, fileStreamProvider)
        {
        }

        protected override Upload CreateFromSpec(Metadata metadata, StreamAdapter streamAdapter)
        {
            return new Upload(metadata.Id, Guid.Empty, 0U, metadata.Name, metadata.ContentType, streamAdapter);
        }

        protected override Metadata CreateMetadata(Upload @in)
        {
            return new Metadata(@in.Id, @in.Name, @in.ContentType, DateTime.UtcNow);
        }

        protected override UploadResultRow CreateSaveResult(Metadata metadata, Upload @in)
        {
            return new UploadResultRow(metadata.Id, @in.Number, metadata.Name, metadata.ContentType);
        }
    }
}
