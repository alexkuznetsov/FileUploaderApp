using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Interfaces;

namespace FileUploadApp.Core.Serialization;

public class Deserializer : IDeserializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public Deserializer()
    {
        _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public TObject? Deserialize<TObject>(string payload) =>
        JsonSerializer.Deserialize<TObject>(payload, _jsonSerializerOptions);

    public async ValueTask<TObject?> DeserializeAsync<TObject>(string file, CancellationToken cancellationToken = default)
    {
        using var f = File.OpenRead(file);
        return await JsonSerializer.DeserializeAsync<TObject>(f, _jsonSerializerOptions, cancellationToken);
    }
}
