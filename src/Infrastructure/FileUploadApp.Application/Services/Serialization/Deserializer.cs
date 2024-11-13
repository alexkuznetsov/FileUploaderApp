using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Interfaces;

namespace FileUploadApp.Application.Services.Serialization;

internal sealed class Deserializer : IDeserializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public Deserializer()
    {
        _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public TObject? DeserializeString<TObject>(string payload) =>
        JsonSerializer.Deserialize<TObject>(payload, _jsonSerializerOptions);

    public async ValueTask<TObject?> DeserializeAsync<TObject>(Stream utf8Json, CancellationToken cancellationToken = default)
    {
        //var fileData = await File.ReadAllTextAsync(file,cancellationToken);
        return await JsonSerializer.DeserializeAsync<TObject>(utf8Json, _jsonSerializerOptions, cancellationToken);
        //var obj = JsonSerializer.Deserialize<TObject>(fileData, _jsonSerializerOptions);

        //return obj;
    }
}
