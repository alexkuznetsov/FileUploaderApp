using FileUploadApp.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class FileAsPlainTextLinkDataPayloadHandler : IPlaintextPayloadHandler<HttpContext>
    {
        public Task ApplyAsync(HttpContext httpContext, IUploadService uploadsService)
        {
            return Task.FromResult(0);
        }
    }
}
