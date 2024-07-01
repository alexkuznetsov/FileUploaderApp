using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Interfaces;

namespace FileUploadApp.Core.Serialization;

public class Serializer : ISerializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public Serializer()
    {
        _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public string Serialize(object @object) =>
        JsonSerializer.Serialize(@object, _jsonSerializerOptions);

    public async Task SerializeAsync<T>(T @object, string file, CancellationToken cancellationToken = default)
    {
        using var stream = File.OpenWrite(file);
        await JsonSerializer.SerializeAsync(stream, @object, _jsonSerializerOptions, cancellationToken);
    }
}
