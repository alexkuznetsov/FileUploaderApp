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
            var request = httpContext.Request;

            if (request.ContentType == JsonContentType)
            {
                var uploadRequest = await httpContext.AssumeAsUploadRequestEvent(deserializer);

                await mediator.Publish(new UploadRequestEvent(uploadRequest));
            }
            else if (request.ContentType == TextPlainContentType)
            {
                var textRequest = await httpContext.AssumeAsPlaintTextRequestEvent();

                await mediator.Publish(new PlainTextRequestEvent(textRequest));
            }
            else if (request.HasFormContentType)
            {
                var filesRequest = await httpContext.AssumeAsFilesRequestEvent();

                await mediator.Publish(new FilesRequestEvent(filesRequest));
            }
            else
                throw new InvalidOperationException("Mailformed request");
        }
    }
}
