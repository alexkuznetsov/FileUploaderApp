using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces;

public interface IDeserializer
{
    TObject? DeserializeString<TObject>(string payload);

    ValueTask<TObject?> DeserializeAsync<TObject>(Stream utf8json, CancellationToken cancellationToken = default);
}
