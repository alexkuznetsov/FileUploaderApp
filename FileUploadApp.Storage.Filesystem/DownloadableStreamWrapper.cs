using System;
using FileUploadApp.Domain;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace FileUploadApp.Storage.Filesystem
{
    internal class DownloadableStreamWrapper : StreamWrapper
    {
        private readonly string pathToFile;

        public DownloadableStreamWrapper(string pathToFile)
        {
            this.pathToFile = pathToFile;
        }

        public override Task<byte[]> AsRawBytesAsync()
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
