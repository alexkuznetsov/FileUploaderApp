using FileUploadApp.Interfaces;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Storage;

public abstract class Store<TKey, TMeta, TIn, TOut> : IStore<TKey, TIn, TOut>
    where TKey : struct
    where TMeta : class
    where TIn : class
{
    private readonly IStoreBackend<TKey, TMeta, TMeta> _metaRepository;
    private readonly IStoreBackend<TKey, TMeta, TIn> _storeBackend;
    private readonly IFileStreamProvider<TKey, Stream> _fileStreamProvider;

    protected Store(IStoreBackend<TKey, TMeta, TMeta> metadataRepository
        , IStoreBackend<TKey, TMeta, TIn> storeBackend
        , IFileStreamProvider<TKey, Stream> fileStreamProvider)
    {
        _metaRepository = metadataRepository;
        this._storeBackend = storeBackend;
        this._fileStreamProvider = fileStreamProvider;
    }

    public async Task<TIn> ReceiveAsync(TKey fileId, CancellationToken cancellationToken = default)
    {
        var spec = await _metaRepository.FindAsync(fileId, cancellationToken).ConfigureAwait(false);

        if (spec == null)
        {
            return default;
        }

        var stream = _fileStreamProvider.GetStream(fileId);

        return CreateFromSpec(spec, stream);
    }

    protected abstract TIn CreateFromSpec(TMeta metadata, Stream streamAdapter);

    protected abstract TMeta CreateMetadata(TIn @in);

    protected abstract TOut CreateSaveResult(TMeta metadata, TIn @in);


    public async Task<TOut> StoreAsync(TIn file, CancellationToken cancellationToken = default)
    {
        var metadata = CreateMetadata(file);

        await _metaRepository.SaveAsync(metadata, cancellationToken).ConfigureAwait(false);
        await _storeBackend.SaveAsync(file, cancellationToken).ConfigureAwait(false);

        return CreateSaveResult(metadata, file);
    }

    public async Task<bool> DeleteAsync(TKey fileId, CancellationToken cancellationToken = default)
    {
        var metadata = await _metaRepository.FindAsync(fileId, cancellationToken).ConfigureAwait(false);
        if (metadata == null) return false;

        await _storeBackend.DeleteAsync(metadata, cancellationToken).ConfigureAwait(false);
        await _metaRepository.DeleteAsync(metadata, cancellationToken).ConfigureAwait(false);

        return true;
    }
}