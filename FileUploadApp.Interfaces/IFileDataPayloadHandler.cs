using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IFileDataPayloadHandler<TPayload>
    {
        Task ApplyAsync(TPayload payload);
    }
}
