using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileUploadApp.Core;
using FileUploadApp.Domain;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Events;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FileUploadApp.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    public class UploadController : BaseApiController
    {
        private readonly IContentTypeTestUtility contentTypeTestUtility;
        private readonly ILogger<UploadController> logger;

        public UploadController(IMediator mediator, IContentTypeTestUtility contentTypeTestUtility, ILogger<UploadController> logger) : base(mediator)
        {
            this.contentTypeTestUtility = contentTypeTestUtility;
            this.logger = logger;
        }

        [HttpPost("")]
        [FormContentType]
        public async Task<IActionResult> PostForm(CancellationToken ct = default)
        {
            var files = await HttpContext.AsUploadFilesEventAsync(contentTypeTestUtility, ct);

            return await UploadCoreAsync(files.ToArray(), ct);
        }

        [HttpPost("")]
        public async Task<IActionResult> PostJson(UploadRequest uploadRequest, CancellationToken ct = default)
        {
            var commands = uploadRequest.AsDownloadUriQueries(e => logger.LogError("Fail to parse URI: {0}", e)).ToArray();
            var files = uploadRequest.AsUploads(contentTypeTestUtility);

            if (!commands.Any()) return await UploadCoreAsync(files.ToArray(), ct);
            
            var newFileTasks = commands.Select(x => SendAsync(x, ct)).ToArray();
            var newFiles = await Task.WhenAll(newFileTasks);

            files = files.Concat(newFiles);

            return await UploadCoreAsync(files.ToArray(), ct);
        }

        private async Task<IActionResult> UploadCoreAsync(Upload[] uploadedFiles, CancellationToken cancellationToken = default)
        {
            if (!uploadedFiles.Any())
                return NotFound();

            var uploadCommand = new UploadFilesEvent(uploadedFiles);
            var receiveQuery = new GetUploadedResultsQuery(uploadedFiles);

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
