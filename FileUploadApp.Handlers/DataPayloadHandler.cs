using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class DataPayloadHandler : IFormJsonPayloadHandler<HttpContext>
    {
        private readonly IFileDataPayloadHandler<FileAsBase64Payload[]> base64DataPayloadHadnler;
        private readonly IFileDataPayloadHandler<string[]> uriDataPayloadHandler;

        public DataPayloadHandler(IFileDataPayloadHandler<FileAsBase64Payload[]> base64DataPayloadHadnler,
                                          IFileDataPayloadHandler<string[]> uriDataPayloadHandler)
        {
            this.base64DataPayloadHadnler = base64DataPayloadHadnler ?? throw new ArgumentNullException(nameof(base64DataPayloadHadnler));
            this.uriDataPayloadHandler = uriDataPayloadHandler ?? throw new ArgumentNullException(nameof(uriDataPayloadHandler));
        }

        public async Task ApplyAsync(HttpContext httpContext, IUploadService uploadService)
        {
            var request = httpContext.Request.EnableRewind();
            var model = await DesirializeAsync(request);

            var isModelOk = model != null;
            var isFilesNotEmpty = isModelOk && model.Files != null;
            var isLinksNotEmpty = isModelOk && model.Links != null;

            if (isFilesNotEmpty)
            {
                await base64DataPayloadHadnler.ApplyAsync(model.Files.ToArray());
            }

            if (isLinksNotEmpty)
            {
                await uriDataPayloadHandler.ApplyAsync(model.Links.ToArray());
            }
        }

        private static async Task<Base64Payload> DesirializeAsync(HttpRequest request)
        {
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                var content = await reader.ReadToEndAsync();

                return JsonConvert.DeserializeObject<Base64Payload>(content);
            }
        }

    }
}
