using System.Threading.Tasks;

namespace FileUploadApp.Core.Serialization
{
    public class Deserializer : Interfaces.IDeserializer
    {
        public Task<TObject> DeserializeAsync<TObject>(string payload)
        {
            var @object = Newtonsoft.Json.JsonConvert.DeserializeObject<TObject>(payload);

            return Task.FromResult(@object);
        }
    }
}
