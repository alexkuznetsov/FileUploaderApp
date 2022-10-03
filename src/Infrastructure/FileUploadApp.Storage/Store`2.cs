using FileUploadApp.Domain;
using System;

namespace FileUploadApp.Storage;

public abstract class Store<TIn, TOut> : Store<Guid, Metadata, TIn, TOut>
    where TIn : class
{
    protected Store(
          IStoreBackend<Guid, Metadata, Metadata> metadataRepository
        , IStoreBackend<Guid, Metadata, TIn> storeBackend
        , IFileStreamProvider<Guid, StreamAdapter> fileStreamProvider)
            : base(metadataRepository, storeBackend, fileStreamProvider)
    {
    }
}
