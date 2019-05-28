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
            _bytea = bytea.ToArray();
        }

        public override Stream Stream
        {
            get
            {
                var str = new MemoryStream();
                str.Write(_bytea.Span);

                return str;
            }
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
