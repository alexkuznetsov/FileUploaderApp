using System.Text.Json;

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
}
