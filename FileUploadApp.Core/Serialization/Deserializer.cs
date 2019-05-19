using FileUploadApp.Interfaces;
using Newtonsoft.Json;

namespace FileUploadApp.Core.Serialization
{
    public class Deserializer : IDeserializer
    {
        public TObject Deserialize<TObject>(string payload) =>
            JsonConvert.DeserializeObject<TObject>(payload);
    }
}
