using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Domain
{
    public abstract class StreamAdapter
    {
        public abstract Stream Stream { get; }

        public abstract Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);

        public abstract Task<ReadOnlyMemory<byte>> AsRawBytesAsync(CancellationToken cancellationToken = default);

        public abstract Task<ReadOnlyMemory<byte>> AsBytesSliceAsync(int len, CancellationToken cancellationToken = default);
    }
}
