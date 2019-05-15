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
        {
            _bytea = bytea;
        }

        public override Stream Stream => new MemoryStream(_bytea.ToArray());

        public override Task<ReadOnlyMemory<byte>> AsRawBytesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_bytea);
        }

        public override Task<ReadOnlyMemory<byte>> AsBytesSlice(int len, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_bytea.Slice(0, len));
        }

        public override Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            return target.WriteAsync(_bytea, cancellationToken)
                .AsTask();
        }
    }
}
