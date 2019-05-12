using System.Collections.Generic;
using System.Threading.Tasks;
using FileUploadApp.Domain;

namespace FileUploadApp.Interfaces
{
    public interface IUploadService
    {
        ICollection<UploadedFile> UploadedFiles { get; set; }

        Task<UploadResult> UploadAsync();
    }
}