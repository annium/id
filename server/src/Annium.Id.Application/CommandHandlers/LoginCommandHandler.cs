using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Application.Commands.Login;
using Annium.Id.Application.Tools;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;
using NodaTime;

namespace Annium.Id.Application.CommandHandlers
{
    internal class LoginCommandHandler : ICommandHandler<LogUserInCommand, Tokens>, ICommandHandler<LogUserOutCommand>, ICommandHandler<UpdateTokensCommand, Tokens>, ICommandHandler<LogUserInAppCommand, Tokens>, ICommandHandler<LogUserOutAppCommand>, ICommandHandler<UpdateUserAppTokenCommand, Tokens>
    {
        private static readonly Duration refreshTokenLifeTime = Duration.FromDays(1);
        private readonly Func<Instant> getInstant;
        private readonly IUserLoginRepository userLoginRepository;
        private readonly IUserAppLoginRepository userAppLoginRepository;
        private readonly ISecurityManager securityManager;
        private readonly IIdentityDataAccessor identityDataAccessor;
        private readonly ITokenGenerator tokenGenerator;

        public LoginCommandHandler(
            Func<Instant> getInstant,
            IUserLoginRepository userLoginRepository,
            IUserAppLoginRepository userAppLoginRepository,
            ISecurityManager securityManager,
            IIdentityDataAccessor identityDataAccessor,
            ITokenGenerator tokenGenerator
        )
        {
            this.getInstant = getInstant;
            this.userLoginRepository = userLoginRepository;
            this.userAppLoginRepository = userAppLoginRepository;
            this.securityManager = securityManager;
            this.identityDataAccessor = identityDataAccessor;
            this.tokenGenerator = tokenGenerator;
        }

        public async Task<IStatusResult<OperationStatus, Tokens>> HandleAsync(
            LogUserInCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = request.User;

            if (securityManager.Hash(request.Password) != user.PasswordHash)
                return Result.Status<OperationStatus, Tokens>(OperationStatus.Forbidden, null).Error("Invalid password");

            var instant = getInstant();
            var(ipAddress, client) = identityDataAccessor.GetIdentityData();
            var login = new UserLogin(user.Id, instant, ipAddress.ToString(), client, Guid.NewGuid(), instant + refreshTokenLifeTime);

            await userLoginRepository.DeleteExpiredByUserIdAsync(user.Id, instant);
            login = await userLoginRepository.CreateAsync(login);

            var token = tokenGenerator.GenerateBaseToken(login);
            var userToken = new Tokens(token, login.RefreshToken, login.RefreshTokenExpires);

            return Result.Status(OperationStatus.OK, userToken);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            LogUserOutCommand request,
            CancellationToken cancellationToken
        )
        {
            await userLoginRepository.DeleteByIdAsync(request.LoginId);

            return Result.Status(OperationStatus.OK);
        }

        public Task<IStatusResult<OperationStatus, Tokens>> HandleAsync(
            UpdateTokensCommand request,
            CancellationToken cancellationToken
        )
        {
            var login = request.Login;

            if (login.RefreshTokenExpires < getInstant())
                return Task.FromResult(Result.Status(OperationStatus.Forbidden, default(Tokens)).Error("Refresh token expired"));

            var token = tokenGenerator.GenerateBaseToken(login);

            return Task.FromResult(Result.Status(OperationStatus.OK, new Tokens(token, login.RefreshToken, login.RefreshTokenExpires)));
        }

        public async Task<IStatusResult<OperationStatus, Tokens>> HandleAsync(
            LogUserInAppCommand request,
            CancellationToken cancellationToken
        )
        {
            var app = request.App;
            var user = request.User;

            if (securityManager.Hash(request.Password) != user.PasswordHash)
                return Result.Status(OperationStatus.Forbidden, default(Tokens)).Error("Invalid password");

            var instant = getInstant();
            var(ipAddress, client) = identityDataAccessor.GetIdentityData();
            var login = new UserAppLogin(app.Id, user.Id, instant, ipAddress.ToString(), client, Guid.NewGuid(), instant + refreshTokenLifeTime);

            await userAppLoginRepository.DeleteExpiredByUserIdAsync(user.Id, instant);
            login = await userAppLoginRepository.CreateAsync(login);

            var token = await tokenGenerator.GenerateAppToken(login);

            return Result.Status(OperationStatus.OK, new Tokens(token, login.RefreshToken, login.RefreshTokenExpires));
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            LogUserOutAppCommand request,
            CancellationToken cancellationToken
        )
        {
            await userAppLoginRepository.DeleteByIdAsync(request.LoginId);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus, Tokens>> HandleAsync(
            UpdateUserAppTokenCommand request,
            CancellationToken cancellationToken
        )
        {
            var app = request.App;
            var login = request.Login;

            if (login.RefreshTokenExpires < getInstant())
                return Result.Status(OperationStatus.Forbidden, default(Tokens)).Error("Refresh token expired");

            var token = await tokenGenerator.GenerateAppToken(login);

            return Result.Status(OperationStatus.OK, new Tokens(token, login.RefreshToken, login.RefreshTokenExpires));
        }
    }
}