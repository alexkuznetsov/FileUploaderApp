using FileUploadApp.Domain;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace FileUploadApp.StreamWrappers
{
    public class DownloadableStreamWrapper : StreamWrapper
    {
        private readonly string pathToFile;

        public DownloadableStreamWrapper(string pathToFile)
        {
            this.pathToFile = pathToFile;
        }

        public override Task<byte[]> AsRawBytesAsync(CancellationToken cancellationToken = default)
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
