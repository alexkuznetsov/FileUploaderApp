using System;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Core.Mvc;
using FileUploadApp.Features.Commands;
using FileUploadApp.Features.Queries;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace FileUploadApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController : BaseApiController
{
    public FileController(IMediator mediator) : base(mediator)
    {

    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any)]
    [SwaggerOperation(
           Summary = "Download a uploaded file by id",
           Description = "Download a file",
           OperationId = "File_Get",
           Tags = ["File"])
       ]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(new DownloadUploadById.Query(id), cancellationToken);

        return response == null
            ? NotFound()
            : File(response.Stream, response.ContentType, response.Name);
    }

    [Authorize]
    [HttpPost("{id}"), HttpDelete("{id}")]
    [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any)]
    [SwaggerOperation(
           Summary = "Delete a file by id",
           Description = "Delete a file",
           OperationId = "File_Delete",
           Tags = ["File"])
    ]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var response = await SendAsync(new DeleteUploadById.Command(id), cancellationToken);

        return response.IsNotFound() ? NotFound() : Ok();
    }
}
