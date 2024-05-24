using System.Text.Json;

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
}
