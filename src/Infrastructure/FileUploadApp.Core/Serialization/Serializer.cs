using FileUploadApp.Interfaces;
using Newtonsoft.Json;

namespace FileUploadApp.Core.Serialization
{
    public class Serializer : ISerializer
    {
        public string Serialize(object @object) =>
            JsonConvert.SerializeObject(@object);
    }
}
