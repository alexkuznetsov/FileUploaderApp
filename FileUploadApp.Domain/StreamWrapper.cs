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

        public abstract Task<byte[]> AsRawBytesAsync(CancellationToken cancellationToken = default);

        public abstract Task<byte[]> AsBytesSliceAsync(int len, CancellationToken cancellationToken = default);

        protected static T[] Slice<T>(T[] source, int from, int len)
        {
            T[] result = new T[len];
            Array.Copy(source, from, result, 0, len);

            return result;
        }
    }
}
