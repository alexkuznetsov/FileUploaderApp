using FileUploadApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FileUploadApp.Handlers
{
    public class PayloadTypeProcessorHelper
    {
        private static readonly string JsonContentType = "application/json";
        private static readonly string TextPlainContentType = "text/plain";

        /// <summary>
        /// Чтобы не внедрять все три сразу, лучше воспользоваться прямым получением из DI
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public IDataPayloadHandler<HttpContext> GetPayloadProcessor(HttpContext httpContext)
        {
            var request = httpContext.Request;

            if (request.ContentType == JsonContentType)
            {
                return httpContext.RequestServices.GetRequiredService<IFormJsonPayloadHandler<HttpContext>>();
            }

            if (request.ContentType == TextPlainContentType)
            {
                return httpContext.RequestServices.GetRequiredService<IPlaintextPayloadHandler<HttpContext>>();
            }

            if (request.HasFormContentType)
            {
                return httpContext.RequestServices.GetRequiredService<IMultipathFormPayloadHandler<HttpContext>>();
            }

            return null;
        }
    }
}
