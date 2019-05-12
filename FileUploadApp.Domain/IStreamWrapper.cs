using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Domain
{
    public interface IStreamWrapper
    {
        Stream GetStream();

        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
    }
}
