using FileUploadApp.Core;
using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : BaseApiController
    {
        private readonly UploadsContext uploadedFilesContext;

        public UploadController(UploadsContext uploadedFilesContext, IMediator mediator) : base(mediator)
        {
            this.uploadedFilesContext = uploadedFilesContext;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(CancellationToken ct = default)
        {
            var files = uploadedFilesContext.YieldAll().ToArray();

            if (!files.Any())
                return NotFound();

            var uploadCommand = new UploadFilesEvent(files);
            var receiveQuery = new GetUploadedResultsQuery(files);

            await PublishAsync(uploadCommand, ct);

            var uploadResult = await SendAsync(receiveQuery, ct);

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
