using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Interfaces;

namespace FileUploadApp.Application.Services.Serialization;
internal sealed class Serializer : ISerializer
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
        //using var stream = File.OpenWrite(file);
        var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes<T>(@object, _jsonSerializerOptions);

        await WriteBytesAsync(file, utf8Bytes, cancellationToken);
    }

    private static async Task WriteBytesAsync(string file, byte[] utf8Bytes, CancellationToken cancellationToken)
    {
        using FileStream stream = File.OpenWrite(file);
        await stream.WriteAsync(utf8Bytes, cancellationToken);
        await stream.FlushAsync(cancellationToken);
        stream.Close();
    }
}
