using FileUploadApp.Domain;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IStorage
    {
        Task<UploadResultRow> StoreAsync(UploadedFile file);

        Task<UploadedFile> ReceiveAsync(string fileId);
    }
}
