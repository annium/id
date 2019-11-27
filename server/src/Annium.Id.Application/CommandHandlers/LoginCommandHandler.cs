using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Application.Commands.Login;
using Annium.Id.Application.Services;
using Annium.Id.Application.Tools;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;
using NodaTime;

namespace Annium.Id.Application.CommandHandlers
{
    internal class LoginCommandHandler :
        ICommandHandler<LogInCommand, Tokens>,
        ICommandHandler<LogOutCommand>,
        ICommandHandler<UpdateTokensCommand, Tokens>
    {
        private readonly Func<Instant> getInstant;
        private readonly IUserLoginRepository userLoginRepository;
        private readonly ISecurityManager securityManager;
        private readonly ILoginService loginService;
        private readonly ITokenGenerator tokenGenerator;

        public LoginCommandHandler(
            Func<Instant> getInstant,
            IUserLoginRepository userLoginRepository,
            ISecurityManager securityManager,
            ILoginService loginService,
            ITokenGenerator tokenGenerator
        )
        {
            this.getInstant = getInstant;
            this.userLoginRepository = userLoginRepository;
            this.securityManager = securityManager;
            this.loginService = loginService;
            this.tokenGenerator = tokenGenerator;
        }

        public async Task<IStatusResult<OperationStatus, Tokens>> HandleAsync(
            LogInCommand request,
            CancellationToken cancellationToken
        )
        {
            var app = request.App;
            var user = request.User;

            if (securityManager.Hash(request.Password) != user.PasswordHash)
                return Result.Status(OperationStatus.Forbidden, default(Tokens)!).Error("Invalid password");

            var tokens = await loginService.LogUserInAsync(app, user);

            return Result.Status(OperationStatus.OK, tokens);
        }

        public async Task<IStatusResult<OperationStatus, Tokens>> HandleAsync(
            UpdateTokensCommand request,
            CancellationToken cancellationToken
        )
        {
            var app = request.App;
            var login = request.Login;

            if (login.RefreshTokenExpires < getInstant())
                return Result.Status(OperationStatus.Forbidden, default(Tokens)!).Error("Refresh token expired");

            var token = await tokenGenerator.GenerateToken(login);

            return Result.Status(OperationStatus.OK, new Tokens(token, login.RefreshToken, login.RefreshTokenExpires));
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            LogOutCommand request,
            CancellationToken cancellationToken
        )
        {
            await userLoginRepository.DeleteByIdAsync(request.LoginId);

            return Result.Status(OperationStatus.OK);
        }
    }
}