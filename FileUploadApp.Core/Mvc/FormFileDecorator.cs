using FileUploadApp.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace FileUploadApp.Core
{
    internal class FormFileDecorator : IFormFileDecorator
    {
        private readonly IFormFile formFile;

        public FormFileDecorator(IFormFile formFile)
        {
            this.formFile = formFile;
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
            => formFile.CopyToAsync(target, cancellationToken);

        public Stream GetStream() => formFile.OpenReadStream();
    }
}
