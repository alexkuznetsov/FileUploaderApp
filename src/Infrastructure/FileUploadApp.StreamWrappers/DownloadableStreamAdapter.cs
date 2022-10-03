using FileUploadApp.Domain;
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

        public override Stream Stream => File.Open(pathToFile
            , FileMode.Open
            , FileAccess.Read
            , FileShare.Read);

        public override Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
            => throw new System.NotSupportedException();
    }
}