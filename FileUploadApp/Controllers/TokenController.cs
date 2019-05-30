using FileUploadApp.Core;
using FileUploadApp.Domain.Authentication;
using FileUploadApp.Requests;
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
    public class TokenController : BaseApiController
    {
        public TokenController(IMediator mediator) : base(mediator)
        {

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]AuthenticationRequest authReq, CancellationToken cancellationToken = default)
        {
            var checkUserResponse = await SendAsync(new CheckUserQuery(authReq.Username, authReq.Password), cancellationToken);

            if (checkUserResponse.UserNotFound())
                return NotFound();

            if (checkUserResponse.UserPasswordMismatch())
                return BadRequest(new { error = "Password is invalid" });

            var userToken = await SendAsync(new CreateTokenQuery(checkUserResponse.User.Username), cancellationToken);

            return Ok(userToken);
        }
    }
}
