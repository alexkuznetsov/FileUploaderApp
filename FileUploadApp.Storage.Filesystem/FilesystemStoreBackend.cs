using FileUploadApp.Domain;
using FileUploadApp.StreamAdapters;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Storage.Filesystem
{
    internal class FilesystemStoreBackend : IStoreBackend<Guid, Upload>, IFileStreamProvider<Guid, StreamAdapter>
    {
        private readonly IPathExpander<Guid> pathExpander;

        public FilesystemStoreBackend(IPathExpander<Guid> pathExpander)
        {
            this.pathExpander = pathExpander;
        }

        public Task<Upload> FindAsync(Guid key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public StreamAdapter GetStreamAdapter(Guid id)
        {
            var path = pathExpander.BuildPathAndCheckDir(id, false);

            return new DownloadableStreamAdapter(path);
        }

        public async Task SaveAsync(Upload upload, CancellationToken cancellationToken = default)
        {
            var filePath = pathExpander.BuildPathAndCheckDir(upload.Id, true);
            using (var wri = File.OpenWrite(filePath))
            {
                await upload.Stream.CopyToAsync(wri, cancellationToken).ConfigureAwait(false);
                await wri.FlushAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
