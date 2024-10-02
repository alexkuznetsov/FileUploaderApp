using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Application.Authentication.Commands;
using FileUploadApp.Application.Authentication.Queries;
using FileUploadApp.Application.Common.Authentication;
using FileUploadApp.Application.Common.Mvc;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace FileUploadApp.Controllers;

/// <summary>
/// Реализация службы аутентификации, для тестирования
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TokenController(IMediator mediator) : ControllerBase
{
    [SwaggerOperation(
       Summary = "Authenticate and recive JWT token for operaions (upload / remove uploaded file)",
       Description = "Authenticate user and issue a jwt token",
       OperationId = "Token_Issue",
       Tags = ["Token"])
    ]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] AuthenticationRequest authReq
        , CancellationToken cancellationToken = default)
    {
        var checkUserResponse = await mediator.Send(new CheckUser.Query(authReq.Username, authReq.Password)
            , cancellationToken);

        if (checkUserResponse.IsNotFound())
            return NotFound();

        if (checkUserResponse.UserPasswordMismatch())
            return BadRequest(new { error = "Password is invalid" });

        var userToken = await mediator.Send(new CreateToken.Command(checkUserResponse.Result.Username)
            , cancellationToken);

        return Ok(userToken);
    }
}
