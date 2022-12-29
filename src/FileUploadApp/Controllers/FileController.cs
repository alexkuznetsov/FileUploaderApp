using FileUploadApp.Core.Mvc;
using FileUploadApp.Features.Commands;
using FileUploadApp.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : BaseApiController
    {
        public FileController(IMediator mediator) : base(mediator)
        {

        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var response = await SendAsync(new DownloadUploadById.Query(id), cancellationToken);

            if (response == null)
                return NotFound();

            return File(response.Stream, response.ContentType, response.Name);
        }

        [HttpPost("{id}"), HttpDelete("{id}")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any)]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var response = await SendAsync(new DeleteUploadById.Command(id), cancellationToken);

            return response.IsNotFound() ? NotFound() : Ok();
        }
    }
}
