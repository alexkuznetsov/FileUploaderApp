using FileUploadApp.Interfaces;
using System.Text.Json;

namespace FileUploadApp.Core.Serialization
{
    public class Deserializer : IDeserializer
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public Deserializer()
        {
            jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        }

        public TObject Deserialize<TObject>(string payload) =>
            JsonSerializer.Deserialize<TObject>(payload, jsonSerializerOptions);
    }
}
