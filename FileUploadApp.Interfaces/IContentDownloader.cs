using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IContentDownloader
    {
        Task<byte[]> Download();
    }

}
