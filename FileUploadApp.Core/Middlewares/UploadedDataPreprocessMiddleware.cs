using Microsoft.AspNetCore.Http;
using System.Threading;
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

        public async Task Invoke(HttpContext httpContext, EventGenerator eventGenerator, CancellationToken cancellationToken = default)
        {
            await eventGenerator.GenerateApprochiateEventAsync(httpContext, cancellationToken);

            await _next(httpContext);
        }
    }
}
