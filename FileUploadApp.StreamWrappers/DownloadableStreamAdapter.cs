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

        public override Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            using (var reader = File.OpenRead(pathToFile))
            {
                return reader.CopyToAsync(target, cancellationToken);
            }
        }
    }
}
