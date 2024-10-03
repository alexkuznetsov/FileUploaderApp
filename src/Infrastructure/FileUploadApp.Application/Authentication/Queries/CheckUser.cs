using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Application.Common;
using FileUploadApp.Application.Common.Messaging;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

namespace FileUploadApp.Application.Authentication.Queries;

public static class CheckUser
{
    public record Query(string Username, string Password) : IMessage<Result>;

    public record Result : ResultBase<Result, User>
    {
        private static readonly int WrongPasswState = 0xaaa;
        public static Result WrongPassword() => new Result { State = WrongPasswState, Result = null! };
        public bool UserPasswordMismatch() => State == WrongPasswState;
    }

    public class Handler : IMessageHandler<Query, Result>
    {
        private readonly ICheckUserService<User> _checkUserService;

        public Handler(ICheckUserService<User> checkUserService)
        {
            _checkUserService = checkUserService;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _checkUserService.FindByNameAsync(request.Username)
                .ConfigureAwait(false);

            if (user == null)
                return Result.NotFound();

            var isPasswordValid = _checkUserService.Authenticate(user, password: request.Password);

            return !isPasswordValid ? Result.WrongPassword() : Result.Ok(user);
        }
    }
}
