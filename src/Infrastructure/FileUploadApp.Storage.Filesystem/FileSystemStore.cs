using FileUploadApp.Domain;

using System;
using System.IO;

namespace FileUploadApp.Storage.Filesystem;

internal sealed class FileSystemStore(IStoreBackend<Guid, Metadata, Metadata> metadataRepository
        , IStoreBackend<Guid, Metadata, Upload> storeBackend
        , IFileStreamProvider<Guid, Stream> fileStreamProvider)
            : Store<Upload, UploadResultRow>(metadataRepository, storeBackend, fileStreamProvider)
{
    protected override Upload CreateFromSpec(Metadata metadata, Stream streamAdapter)
        => new(metadata.Id, Guid.Empty, 0U, metadata.Name, metadata.ContentType, streamAdapter);

    protected override Metadata CreateMetadata(Upload @in)
        => new(@in.Id, @in.Name, @in.ContentType, DateTime.UtcNow);

    protected override UploadResultRow CreateSaveResult(Metadata metadata, Upload @in)
        => new(metadata.Id, @in.Number, metadata.Name, metadata.ContentType);
}
