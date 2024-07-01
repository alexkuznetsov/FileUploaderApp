using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces;

public interface IDeserializer
{
    TObject? Deserialize<TObject>(string payload);

    ValueTask<TObject?> DeserializeAsync<TObject>(string file, CancellationToken cancellationToken = default);
}
