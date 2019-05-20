using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Storage
{
    public abstract class Store<TIn, TOut> : IStore<TIn, TOut>
    {
        private readonly IStoreBackend<Guid, Metadata> metaRepository;
        private readonly IStoreBackend<Guid, TIn> storeBackend;
        private readonly IFileStreamProvider<Guid, StreamAdapter> fileStreamProvider;

        protected Store(IStoreBackend<Guid, Metadata> metadataRepository, IStoreBackend<Guid, TIn> storeBackend, IFileStreamProvider<Guid, StreamAdapter> fileStreamProvider)
        {
            this.metaRepository = metadataRepository;
            this.storeBackend = storeBackend;
            this.fileStreamProvider = fileStreamProvider;
        }

        public async Task<TIn> ReceiveAsync(Guid fileId, CancellationToken cancellationToken = default)
        {
            var spec = await metaRepository.FindAsync(fileId, cancellationToken).ConfigureAwait(false);

            if (spec == null)
            {
                return default;
            }

            var streamAdapter = fileStreamProvider.GetStreamAdapter(fileId);

            return CreateFromSpec(spec, streamAdapter);
        }

        protected abstract TIn CreateFromSpec(Metadata metadata, StreamAdapter streamAdapter);

        protected abstract Metadata CreateMetadata(TIn @in);

        protected abstract TOut CreateSaveResult(Metadata metadata, TIn @in);


        public async Task<TOut> StoreAsync(TIn file, CancellationToken cancellationToken = default)
        {
            var metadata = CreateMetadata(file);

            await metaRepository.SaveAsync(metadata, cancellationToken).ConfigureAwait(false);
            await storeBackend.SaveAsync(file, cancellationToken);

            return CreateSaveResult(metadata, file);
        }
    }
}
