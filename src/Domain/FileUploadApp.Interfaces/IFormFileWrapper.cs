using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IFormFileDecorator
    {
        Stream GetStream();
        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
    }
}
