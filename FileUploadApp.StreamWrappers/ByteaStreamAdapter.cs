using FileUploadApp.Domain;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.StreamAdapters
{
    public class ByteaStreamAdapter : StreamAdapter
    {
        private readonly byte[] _bytea;

        public ByteaStreamAdapter(byte[] bytea)
        {
            _bytea = bytea;
        }

        public override Stream Stream => new MemoryStream(_bytea);

        public override Task<byte[]> AsRawBytesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_bytea);
        }

        public override Task<byte[]> AsBytesSliceAsync(int len, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Slice(_bytea, 0, len));
        }

        public override async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            using (Stream)
            {
                await Stream.CopyToAsync(target, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
