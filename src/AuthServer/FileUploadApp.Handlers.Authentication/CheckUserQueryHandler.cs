﻿using FileUploadApp.Domain;
using FileUploadApp.Domain.Authentication;
using FileUploadApp.Interfaces.Authentication;
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
            var user = await checkUserService.FindByNameAsync(request.Username)
                .ConfigureAwait(false);

            if (user == null)
                return CheckUserResult.NotFound;

            var isPasswordValid = checkUserService.Authenticate(user, password: request.Password);

            return !isPasswordValid ? CheckUserResult.WrongPassword : CheckUserResult.Ok(user);
        }
    }
}
