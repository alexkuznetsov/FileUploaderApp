using System;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IContentDownloader
    {
        Task<ReadOnlyMemory<byte>> Download();
    }

}
