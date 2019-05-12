using FileUploadApp.Domain;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Core.Streams
{
    public class FormFileStreamWrapper : StreamWrapper
    {
        private readonly IFormFile formFile;

        public FormFileStreamWrapper(IFormFile formFile)
        {
            this.formFile = formFile;
        }

        public override async Task<byte[]> AsRawBytesAsync()
        {
            using (var s = new MemoryStream())
            {
                await CopyToAsync(s);
                return s.ToArray();
            }
        }

        public override Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
            => formFile.CopyToAsync(target, cancellationToken);

        //public override Stream GetStream()
        //    => formFile.OpenReadStream();
    }
}
