using FileUploadApp.Authentication;
using FileUploadApp.Authentication.Commands;
using FileUploadApp.Authentication.Queries;
using FileUploadApp.Core.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Controllers
{
    /// <summary>
    /// Реализация службы аутентификации, для тестирования
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseApiController
    {
        public TokenController(IMediator mediator) : base(mediator)
        {

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]AuthenticationRequest authReq
            , CancellationToken cancellationToken = default)
        {
            var checkUserResponse = await SendAsync(new CheckUser.Query(authReq.Username, authReq.Password)
                , cancellationToken);

            if (checkUserResponse.UserNotFound())
                return NotFound();

            if (checkUserResponse.UserPasswordMismatch())
                return BadRequest(new { error = "Password is invalid" });

            var userToken = await SendAsync(new CreateToken.Command(checkUserResponse.User.Username)
                , cancellationToken);

            return Ok(userToken);
        }
    }
}
