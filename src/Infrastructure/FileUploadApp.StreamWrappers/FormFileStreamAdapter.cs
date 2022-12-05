using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.StreamAdapters
{
    public class FormFileStreamAdapter : StreamAdapter
    {
        private readonly IFormFileDecorator formFile;

        public override bool ShouldBeDisposed => true;

        public FormFileStreamAdapter(IFormFileDecorator formFile) => this.formFile = formFile;

        public override Stream Stream => formFile.GetStream();

        public override async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
            => await formFile.CopyToAsync(target, cancellationToken).ConfigureAwait(false);
    }
}
