using System;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IDownloadHelper
    {
        Task Download(Uri u);
    }
}