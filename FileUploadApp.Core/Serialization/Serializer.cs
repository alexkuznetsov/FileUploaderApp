using System.Threading.Tasks;

namespace FileUploadApp.Core.Serialization
{
    public class Serializer : Interfaces.ISerializer
    {
        public Task<string> SerializeAsync(object @object)
        {
            var text = Newtonsoft.Json.JsonConvert.SerializeObject(@object);

            return Task.FromResult(text);
        }
    }
}
