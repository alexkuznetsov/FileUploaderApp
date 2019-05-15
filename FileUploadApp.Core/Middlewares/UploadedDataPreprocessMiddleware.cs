using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileUploadApp.Core.Middlewares
{
    public class UploadedDataPreprocessMiddleware
    {
        private readonly RequestDelegate _next;

        public UploadedDataPreprocessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, EventGenerator eventGenerator)
        {
            await eventGenerator.GenerateApprochiateEvent(httpContext);

            await _next(httpContext);
        }
    }
}
