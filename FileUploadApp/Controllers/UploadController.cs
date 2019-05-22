using FileUploadApp.Core;
using FileUploadApp.Domain;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Events;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Controllers
{

    [Route("api/[controller]")]
    public class UploadController : BaseApiController
    {
        private readonly IContentTypeTestUtility contentTypeTestUtility;

        public UploadController(IMediator mediator, IContentTypeTestUtility contentTypeTestUtility) : base(mediator)
        {
            this.contentTypeTestUtility = contentTypeTestUtility;
        }

        [HttpPost("")]
        [FormContentType]
        public async Task<IActionResult> PostForm(CancellationToken ct = default)
        {
            var files = await HttpContext.AsUploadFilesEventAsync(contentTypeTestUtility, ct);

            if (!files.Any())
                return NotFound();

            return await UploadCoreAsync(files);
        }

        [HttpPost("")]
        public async Task<IActionResult> PostJson(UploadRequest uploadRequest, CancellationToken ct = default)
        {
            var commands = uploadRequest.AsDownloadUriQueries().ToArray();
            var files = uploadRequest.AsUploads(contentTypeTestUtility);

            if (commands.Any())
            {
                var newFileTasks = commands.Select(x => SendAsync(x, ct)).ToArray();
                var newFiels = await Task.WhenAll(newFileTasks);

                files = files.Concat(newFiels);
            }

            return await UploadCoreAsync(files.ToArray(), ct);
        }

        private async Task<IActionResult> UploadCoreAsync(IEnumerable<Upload> files, CancellationToken cancellationToken = default)
        {
            var uploadCommand = new UploadFilesEvent(files);
            var receiveQuery = new GetUploadedResultsQuery(files);

            await PublishAsync(uploadCommand, cancellationToken);

            var uploadResult = await SendAsync(receiveQuery, cancellationToken);

            return Ok(uploadResult.Result.Select(x => new
            {
                number = x.Number,
                name = x.Name,
                fileId = x.Id,
                previewId = x.Preview?.Id
            }).ToArray());
        }
    }
}
