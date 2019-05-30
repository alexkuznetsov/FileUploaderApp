using FileUploadApp.Domain;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.StreamAdapters
{
    public class ByteaStreamAdapter : StreamAdapter
    {
        private readonly ReadOnlyMemory<byte> _bytea;

        public ByteaStreamAdapter(ReadOnlyMemory<byte> bytea)
            => _bytea = bytea;

        public override Stream Stream
            => new MemoryStream(_bytea.ToArray());

        public override async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
            => await target.WriteAsync(_bytea, cancellationToken).ConfigureAwait(false);
    }
}
