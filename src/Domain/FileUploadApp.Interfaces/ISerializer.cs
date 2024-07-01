using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces;

public interface ISerializer
{
    string Serialize(object @object);

    Task SerializeAsync<T>(T @object, string file, CancellationToken cancellationToken = default);
}
