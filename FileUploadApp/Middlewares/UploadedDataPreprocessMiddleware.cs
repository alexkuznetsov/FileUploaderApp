using FileUploadApp.Handlers;
using FileUploadApp.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileUploadApp.Middlewares
{
    internal class UploadedDataPreprocessMiddleware
    {
        private readonly RequestDelegate _next;

        public UploadedDataPreprocessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IUploadService uploadsService, PayloadTypeProcessorHelper processorHelper)
        {
            var proc = processorHelper.GetPayloadProcessor(httpContext);

            await proc?.ApplyAsync(httpContext, uploadsService);

            await _next(httpContext);
        }
    }
}
