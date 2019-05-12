using FileUploadApp.Core.Streams;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class FormDataPayloadHandler : IMultipathFormPayloadHandler<HttpContext>
    {
        private readonly IContentTypeTestUtility contentTypeChecker;

        public FormDataPayloadHandler(IContentTypeTestUtility contentTypeChecker)
        {
            this.contentTypeChecker = contentTypeChecker;
        }

        public async Task ApplyAsync(HttpContext httpContext, IUploadService uploadsService)
        {
            var form = await httpContext.Request.ReadFormAsync();

            foreach (var file in form.Files)
            {
                if (contentTypeChecker.IsAllowed(file.ContentType))
                {
                    uploadsService.UploadedFiles.Add(new UploadedFile(
                        name: file.FileName,
                        contentType: file.ContentType,
                        height: 0,
                        width: 0,
                        streamWrapper: new FormFileStreamWrapper(file)));
                }
            }
        }
    }
}
