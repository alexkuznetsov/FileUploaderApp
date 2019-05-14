using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.StreamWrappers
{
    public class FormFileStreamWrapper : StreamWrapper
    {
        private readonly IFormFileWrapper formFile;

        public FormFileStreamWrapper(IFormFileWrapper formFile)
        {
            this.formFile = formFile;
        }

        public override async Task<byte[]> AsRawBytesAsync(CancellationToken cancellationToken = default)
        {
            using (var s = new MemoryStream())
            {
                await CopyToAsync(s, cancellationToken);
                return s.ToArray();
            }
        }

        public override Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
            => formFile.CopyToAsync(target, cancellationToken);

        //public override Stream GetStream()
        //    => formFile.OpenReadStream();
    }
}
