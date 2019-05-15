using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.StreamAdapters
{
    public class FormFileStreamAdapter : StreamAdapter
    {
        private readonly IFormFileDecorator formFile;

        public FormFileStreamAdapter(IFormFileDecorator formFile)
        {
            this.formFile = formFile;
        }

        public override Task<ReadOnlyMemory<byte>> AsBytesSlice(int len, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Stream Stream
        {
            get
            {
                return formFile.GetStream();
            }
        }

        public override async Task<ReadOnlyMemory<byte>> AsRawBytesAsync(CancellationToken cancellationToken = default)
        {
            using (var s = new MemoryStream())
            {
                await CopyToAsync(s, cancellationToken);
                return s.ToArray().AsMemory();
            }
        }

        public override Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
            => formFile.CopyToAsync(target, cancellationToken);
    }
}
