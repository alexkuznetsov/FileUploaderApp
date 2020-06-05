using FileUploadApp.Domain;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.StreamAdapters
{
    public class ByteaStreamAdapter : StreamAdapter
    {
        private readonly ReadOnlyMemory<byte> _byteArray;

        public ByteaStreamAdapter(ReadOnlyMemory<byte> byteArray)
            => _byteArray = byteArray;

        public override Stream Stream
            => new MemoryStream(_byteArray.Span.ToArray());

        public override async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
            => await target.WriteAsync(_byteArray, cancellationToken).ConfigureAwait(false);
    }
}
