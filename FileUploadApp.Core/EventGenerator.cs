using FileUploadApp.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApp.Core
{
    public class EventGenerator
    {
        private static readonly string JsonContentType = "application/json";
        private static readonly string TextPlainContentType = "text/plain";

        private readonly IMediator mediator;

        public EventGenerator(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task GenerateApprochiateEvent(HttpContext httpContext)
        {
            var events = await CreateEventAsync(httpContext).ConfigureAwait(false);
            var task = events.Select(x => mediator.Publish(x))
                .ToArray();

            await Task.WhenAll(task);
        }

        private async Task<IEnumerable<GenericEvent>> CreateEventAsync(HttpContext httpContext)
        {
            var request = httpContext.Request;

            if (request.ContentType == JsonContentType)
            {
                return await httpContext.AssumeAsUploadRequestEvents().ConfigureAwait(false);
            }
            else if (request.ContentType == TextPlainContentType)
            {
                return await httpContext.AssumeAsPlaintTextRequestEvents().ConfigureAwait(false);
            }
            else if (request.HasFormContentType)
            {
                return await httpContext.AssumeAsFilesRequestEvents().ConfigureAwait(false);
            }
            else
                throw new InvalidOperationException("Mailformed request");
        }
    }
}
