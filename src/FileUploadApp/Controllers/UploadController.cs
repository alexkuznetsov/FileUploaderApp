using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Core.Mvc;
using FileUploadApp.Domain;
using FileUploadApp.Domain.Raw;
using FileUploadApp.Features;
using FileUploadApp.Features.Commands;
using FileUploadApp.Features.Queries;
using FileUploadApp.Interfaces;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Swashbuckle.AspNetCore.Annotations;

namespace FileUploadApp.Controllers;


[Route("api/[controller]")]
[Authorize]
[ApiController]
public class UploadController : BaseApiController
{
    private readonly IContentTypeTestUtility _contentTypeTestUtility;
    private readonly ILogger<UploadController> _logger;

    public UploadController(IMediator mediator
        , IContentTypeTestUtility contentTypeTestUtility
        , ILogger<UploadController> logger) : base(mediator)
    {
        this._contentTypeTestUtility = contentTypeTestUtility;
        this._logger = logger;
    }

    [HttpPost(Name = "PostFormData")]
    [FormContentType]
    [SwaggerOperation(
        Summary = "Upload files from FormData",
        Description = "Upload files",
        OperationId = "Upload_FormData",
        Tags = ["Upload"])
    ]
    public async Task<IActionResult> PostForm(CancellationToken ct = default)
    {
        var files = await HttpContext.AsUploadFilesEventAsync(_contentTypeTestUtility, ct);

        return await UploadCoreAsync(files.ToArray(), ct);
    }

    [HttpPost(Name = "PostJSON")]
    [SwaggerOperation(
        Summary = "Upload files from JSON payload",
        Description = "Upload files",
        OperationId = "Upload_JSON",
        Tags = ["Upload"])
    ]
    public async Task<IActionResult> PostJson(UploadRequest uploadRequest, CancellationToken ct = default)
    {
        var commands = uploadRequest.AsDownloadUriQueries(
            e => _logger.LogError("Fail to parse URI: {Error}", e)).ToArray();
        var files = uploadRequest.AsUploads(_contentTypeTestUtility);

        if (commands.Length == 0)
        {
            return await UploadCoreAsync(files.ToArray(), ct);
        }

        var newFileTasks = commands.Select(x => SendAsync(x, ct)).ToArray();
        var newFiles = await Task.WhenAll(newFileTasks);

        files = files.Concat(newFiles);

        return await UploadCoreAsync(files.ToArray(), ct);
    }

    /// <summary>
    /// Upload handling
    /// </summary>
    /// <param name="uploadedFiles"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [NonAction]
    private async Task<IActionResult> UploadCoreAsync(Upload[] uploadedFiles
        , CancellationToken cancellationToken = default)
    {
        if (uploadedFiles.Length == 0)
            return NotFound();

        var uploadCommand = new UploadFiles.Event(uploadedFiles);
        var receiveQuery = new GetUploadedResults.Query(uploadedFiles);

        await PublishAsync(uploadCommand, cancellationToken);

        var uploadResult = await SendAsync(receiveQuery, cancellationToken);

        return Ok(uploadResult.Result.Where(x => x != null).Select(x => new
        {
            number = x!.Number,
            name = x.Name,
            fileId = x.Id,
            previewId = x.Preview?.Id
        }).ToArray());
    }
}
