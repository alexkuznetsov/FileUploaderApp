using FileUploadApp.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Authentication.Queries;

public class CheckUser
{
    public class Query : IRequest<Result>
    {
        public Query(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }
        public string Password { get; }
    }

    public class Result
    {
        public static readonly Result NotFound = new(null);

        public static readonly Result WrongPassword = new(null);

        public static Result Ok(User user) => new(user);

        private Result(User user)
        {
            User = user;
        }

        public User User { get; }

        public bool UserNotFound() => ReferenceEquals(this, NotFound);
        public bool UserPasswordMismatch() => ReferenceEquals(this, WrongPassword);
    }

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly ICheckUserService<User> checkUserService;

        public Handler(ICheckUserService<User> checkUserService)
        {
            this.checkUserService = checkUserService;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await checkUserService.FindByNameAsync(request.Username)
                .ConfigureAwait(false);

            if (user == null)
                return Result.NotFound;

            var isPasswordValid = checkUserService.Authenticate(user, password: request.Password);

            return !isPasswordValid ? Result.WrongPassword : Result.Ok(user);
        }
    }
}
