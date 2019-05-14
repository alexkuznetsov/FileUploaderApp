using FileUploadApp.Events;
using FileUploadApp.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FileUploadApp.Core
{
    public class EventGenerator
    {
        private static readonly string JsonContentType = "application/json";
        private static readonly string TextPlainContentType = "text/plain";

        private readonly IDeserializer deserializer;
        private readonly IMediator mediator;

        public EventGenerator(IDeserializer deserializer, IMediator mediator)
        {
            this.deserializer = deserializer;
            this.mediator = mediator;
        }

        public async Task GenerateApprochiateEvent(HttpContext httpContext)
        {
            var @event = await CreateEventAsync(httpContext).ConfigureAwait(false);

            await mediator.Publish(@event).ConfigureAwait(false);
        }

        private async Task<GenericEvent> CreateEventAsync(HttpContext httpContext)
        {
            var request = httpContext.Request;

            if (request.ContentType == JsonContentType)
            {
                return await httpContext.AssumeAsUploadRequestEvent(deserializer).ConfigureAwait(false);
            }
            else if (request.ContentType == TextPlainContentType)
            {
                return await httpContext.AssumeAsPlaintTextRequestEvent().ConfigureAwait(false);
            }
            else if (request.HasFormContentType)
            {
                return await httpContext.AssumeAsFilesRequestEvent().ConfigureAwait(false);
            }
            else
                throw new InvalidOperationException("Mailformed request");
        }
    }
}
