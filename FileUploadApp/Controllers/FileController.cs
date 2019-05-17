using FileUploadApp.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace FileUploadApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IMediator mediator;

        public FileController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Get(string id)
        {
            var response = await mediator.Send(new DownloadUploadByIdQuery(id))
                .ConfigureAwait(false);

            if (response == null)
                throw new FileNotFoundException();

            return new FileStreamResult(response.Stream.Stream, response.ContentType);

            //using (Response.Body)
            //{
            //    Response.StatusCode = 200;
            //    Response.ContentType = response.ContentType;

            //    await response.Stream.CopyToAsync(Response.Body);

            //    //return File(stream, response.ContentType, response.Name);
            //}
        }
    }
}
