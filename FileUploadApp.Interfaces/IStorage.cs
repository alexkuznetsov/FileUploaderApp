using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IStorage<TIn, TOut>
    {
        Task<TOut> StoreAsync(TIn file);

        Task<TIn> ReceiveAsync(string fileId);
    }
}
