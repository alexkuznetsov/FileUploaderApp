using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IFormFileDecorator
    {
        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
    }
}
