using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUploadApp.Requests;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using FileUploadApp.Events;

namespace FileUploadApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly UploadsContext uploadedFilesContext;
        private readonly IMediator mediator;

        public UploadController(UploadsContext uploadedFilesContext, IMediator mediator)
        {
            this.uploadedFilesContext = uploadedFilesContext;
            this.mediator = mediator;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(CancellationToken ct = default(CancellationToken))
        {
            var files = uploadedFilesContext.GetList();
            var uploadCommand = new UploadFilesEvent(files);
            var receiveQuery = new GetUploadedResultsQuery(files);

            await mediator.Publish(uploadCommand, ct);

            var uploadResult = await mediator.Send(receiveQuery, ct);

            return Ok(uploadResult.Result.Select(x => new
            {
                number = x.Number,
                name = x.Name,
                fileId = x.Id,
                previewId = x.Preview.Id
            }).ToArray());
        }
    }
}
