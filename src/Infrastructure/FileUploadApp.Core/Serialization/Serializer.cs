using FileUploadApp.Interfaces;
using System.Text.Json;

namespace FileUploadApp.Core.Serialization
{
    public class Serializer : ISerializer
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public Serializer()
        {
            jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        }

        public string Serialize(object @object) =>
            JsonSerializer.Serialize(@object, jsonSerializerOptions);
    }
}
