using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Storage;

public abstract class Store<TKey, TMeta, TIn, TOut> : IStore<TKey, TIn, TOut>
    where TKey : struct
    where TMeta : class
    where TIn : class
{
    private readonly IStoreBackend<TKey, TMeta> metaRepository;
    private readonly IStoreBackend<TKey, TIn> storeBackend;
    private readonly IFileStreamProvider<TKey, StreamAdapter> fileStreamProvider;

    protected Store(IStoreBackend<TKey, TMeta> metadataRepository
        , IStoreBackend<TKey, TIn> storeBackend
        , IFileStreamProvider<TKey, StreamAdapter> fileStreamProvider)
    {
        metaRepository = metadataRepository;
        this.storeBackend = storeBackend;
        this.fileStreamProvider = fileStreamProvider;
    }

    public async Task<TIn> ReceiveAsync(TKey fileId, CancellationToken cancellationToken = default)
    {
        var spec = await metaRepository.FindAsync(fileId, cancellationToken).ConfigureAwait(false);

        if (spec == null)
        {
            return default;
        }

        var streamAdapter = fileStreamProvider.GetStreamAdapter(fileId);

        return CreateFromSpec(spec, streamAdapter);
    }

    protected abstract TIn CreateFromSpec(TMeta metadata, StreamAdapter streamAdapter);

    protected abstract TMeta CreateMetadata(TIn @in);

    protected abstract TOut CreateSaveResult(TMeta metadata, TIn @in);


    public async Task<TOut> StoreAsync(TIn file, CancellationToken cancellationToken = default)
    {
        var metadata = CreateMetadata(file);

        await metaRepository.SaveAsync(metadata, cancellationToken).ConfigureAwait(false);
        await storeBackend.SaveAsync(file, cancellationToken).ConfigureAwait(false);

        return CreateSaveResult(metadata, file);
    }

    public async Task<bool> DeleteAsync(TKey fileId, CancellationToken cancellationToken)
    {
        var metadata = await metaRepository.FindAsync(fileId, cancellationToken).ConfigureAwait(false);
        if (metadata == null) return false;

        await storeBackend.DeleteAsync(fileId, cancellationToken).ConfigureAwait(false);
        await metaRepository.DeleteAsync(fileId, cancellationToken).ConfigureAwait(false);

        return true;
    }
}