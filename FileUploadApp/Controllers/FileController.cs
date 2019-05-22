using FileUploadApp.Core;
using FileUploadApp.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await SendAsync(new DownloadUploadByIdQuery(id));

            if (response == null)
                return NotFound();

            return File(response.Stream.Stream, response.ContentType);
        }
    }
}
