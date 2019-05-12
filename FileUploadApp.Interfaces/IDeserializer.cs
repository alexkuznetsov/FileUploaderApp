using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IDeserializer
    {
        Task<TObject> DeserializeAsync<TObject>(string payload);
    }
}
