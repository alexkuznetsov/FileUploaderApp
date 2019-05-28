using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class CheckUserQueryHandler : IRequestHandler<CheckUserQuery, CheckUserResult>
    {
        private readonly ICheckUserService<User> checkUserService;

        public CheckUserQueryHandler(ICheckUserService<User> checkUserService)
        {
            this.checkUserService = checkUserService;
        }

        public async Task<CheckUserResult> Handle(CheckUserQuery request, CancellationToken cancellationToken)
        {
            var user = await checkUserService.FindByNameAsync(request.Username);

            if (user == null)
                return CheckUserResult.NotFound;

            var isPasswValid = checkUserService.Authenticate(user, password: request.Password);

            if (!isPasswValid)
                return CheckUserResult.WrongPassw;

            return CheckUserResult.Ok(user);
        }
    }
}
