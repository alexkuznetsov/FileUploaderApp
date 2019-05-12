using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface ISerializer
    {
        Task<string> SerializeAsync(object @object);
    }
}
