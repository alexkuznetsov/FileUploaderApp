using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUploadApp.Services
{
    public class UploadService : IUploadService
    {
        public ICollection<IUploadedFile> UploadedFiles { get; set; } = new List<IUploadedFile>();

        public Task<UploadResult> UploadAsync()
        {
            return Task.FromResult(new UploadResult { });
        }
    }
}
