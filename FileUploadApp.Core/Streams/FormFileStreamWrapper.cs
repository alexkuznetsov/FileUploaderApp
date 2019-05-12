using FileUploadApp.Domain;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Core.Streams
{
    public class FormFileStreamWrapper : IStreamWrapper
    {
        private readonly IFormFile formFile;

        public FormFileStreamWrapper(IFormFile formFile)
        {
            this.formFile = formFile;
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
            => formFile.CopyToAsync(target, cancellationToken);

        public Stream GetStream()
            => formFile.OpenReadStream();
    }
}
