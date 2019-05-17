using FileUploadApp.Core;
using FileUploadApp.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileUploadApp.Controllers
{
    [Route("api/[controller]")]
    public class FileController : BaseApiController
    {
        public FileController(IMediator mediator) : base(mediator)
        {

        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Get(string id)
        {
            var response = await SendAsync(new DownloadUploadByIdQuery(id))
                .ConfigureAwait(false);

            if (response == null)
                return NotFound();

            return File(response.Stream.Stream, response.ContentType);
        }
    }
}
