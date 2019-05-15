using FileUploadApp.Domain;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace FileUploadApp.StreamAdapters
{
    public class DownloadableStreamAdapter : StreamAdapter
    {
        private readonly string pathToFile;

        public DownloadableStreamAdapter(string pathToFile)
        {
            this.pathToFile = pathToFile;
        }

        public override Task<ReadOnlyMemory<byte>> AsBytesSlice(int len, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<ReadOnlyMemory<byte>> AsRawBytesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Stream Stream => File.OpenRead(pathToFile);

        public override async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            using (Stream)
            {
                await Stream.CopyToAsync(target, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
