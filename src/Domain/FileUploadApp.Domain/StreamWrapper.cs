using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Domain;

public abstract class StreamAdapter
{
    public virtual bool ShouldBeDisposed => false;
    public abstract Stream Stream { get; }

    public abstract Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
}
