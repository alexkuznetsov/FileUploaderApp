using System.Collections.Generic;
using System.Threading.Tasks;
using FileUploadApp.Domain;

namespace FileUploadApp.Interfaces
{
    public interface IUploadService
    {
        ICollection<IUploadedFile> UploadedFiles { get; set; }

        Task<UploadResult> UploadAsync();
    }
}