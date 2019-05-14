using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Domain
{
    public abstract class StreamWrapper
    {
        public abstract Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);

        public abstract Task<byte[]> AsRawBytesAsync(CancellationToken cancellationToken = default);
    }
}
