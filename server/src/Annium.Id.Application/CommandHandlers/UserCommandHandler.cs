using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Application.Commands.Users;
using Annium.Id.Application.Tools;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;
using NodaTime;

namespace Annium.Id.Application.CommandHandlers
{
    internal class UserCommandHandler : ICommandHandler<CreateUserCommand, Guid>, ICommandHandler<UpdateUserCommand>, ICommandHandler<DeleteUserCommand>, ICommandHandler<LogUserInCommand, UserToken>, ICommandHandler<LogUserOutCommand>, ICommandHandler<UpdateUserTokenCommand, UserToken>, ICommandHandler<LogUserInAppCommand, UserToken>, ICommandHandler<LogUserOutAppCommand>, ICommandHandler<UpdateUserAppTokenCommand, UserToken>
    {
        private static readonly Duration refreshTokenLifeTime = Duration.FromDays(1);
        private readonly Func<Instant> getInstant;
        private readonly IUserRepository userRepository;
        private readonly IUserLoginRepository userLoginRepository;
        private readonly IUserAppLoginRepository userAppLoginRepository;
        private readonly ISecurityManager securityManager;
        private readonly IIdentityDataAccessor identityDataAccessor;
        private readonly ITokenGenerator tokenGenerator;

        public UserCommandHandler(
            Func<Instant> getInstant,
            IUserRepository userRepository,
            IUserLoginRepository userLoginRepository,
            IUserAppLoginRepository userAppLoginRepository,
            ISecurityManager securityManager,
            IIdentityDataAccessor identityDataAccessor,
            ITokenGenerator tokenGenerator
        )
        {
            this.getInstant = getInstant;
            this.userRepository = userRepository;
            this.userLoginRepository = userLoginRepository;
            this.userAppLoginRepository = userAppLoginRepository;
            this.securityManager = securityManager;
            this.identityDataAccessor = identityDataAccessor;
            this.tokenGenerator = tokenGenerator;
        }

        public async Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
            CreateUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var passwordHash = securityManager.Hash(request.Password);

            var user = new User(
                request.Login,
                passwordHash,
                request.Email
            );

            user = await userRepository.CreateAsync(user);

            return Result.Status(OperationStatus.OK, user.Id);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            UpdateUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = request.User;

            if (request.Login != user.Login && (await userRepository.FindByLoginAsync(request.Login)) != null)
                return Result.Status(OperationStatus.Conflict).Error($"Login {request.Login} is already used");

            if (request.Email != user.Email && (await userRepository.FindByEmailAsync(request.Email)) != null)
                return Result.Status(OperationStatus.Conflict).Error($"Email {request.Email} is already used");

            user.Login = request.Login;
            user.PasswordHash = securityManager.Hash(request.Password);
            user.Email = request.Email;

            await userRepository.UpdateAsync(user);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            DeleteUserCommand request,
            CancellationToken cancellationToken
        )
        {
            await userLoginRepository.DeleteAllByUserIdAsync(request.MyId);
            await userRepository.DeleteByIdAsync(request.MyId);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus, UserToken>> HandleAsync(
            LogUserInCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = request.User;

            if (securityManager.Hash(request.Password) != user.PasswordHash)
                return Result.Status<OperationStatus, UserToken>(OperationStatus.Forbidden, null).Error("Invalid password");

            var instant = getInstant();
            var(ipAddress, client) = identityDataAccessor.GetIdentityData();
            var login = new UserLogin(user.Id, instant, ipAddress.ToString(), client, Guid.NewGuid(), instant + refreshTokenLifeTime);

            await userLoginRepository.DeleteExpiredByUserIdAsync(user.Id, instant);
            login = await userLoginRepository.CreateAsync(login);

            var token = tokenGenerator.GenerateBaseToken(login);
            var userToken = new UserToken(token, login.RefreshToken, login.RefreshTokenExpires);

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

        public Task<IStatusResult<OperationStatus, UserToken>> HandleAsync(
            UpdateUserTokenCommand request,
            CancellationToken cancellationToken
        )
        {
            var login = request.Login;

            if (login.RefreshTokenExpires < getInstant())
                return Task.FromResult(Result.Status(OperationStatus.Forbidden, default(UserToken)).Error("Refresh token expired"));

            var token = tokenGenerator.GenerateBaseToken(login);

            return Task.FromResult(Result.Status(OperationStatus.OK, new UserToken(token, login.RefreshToken, login.RefreshTokenExpires)));
        }

        public async Task<IStatusResult<OperationStatus, UserToken>> HandleAsync(
            LogUserInAppCommand request,
            CancellationToken cancellationToken
        )
        {
            var app = request.App;
            var user = request.User;

            if (securityManager.Hash(request.Password) != user.PasswordHash)
                return Result.Status(OperationStatus.Forbidden, default(UserToken)).Error("Invalid password");

            var instant = getInstant();
            var(ipAddress, client) = identityDataAccessor.GetIdentityData();
            var login = new UserAppLogin(app.Id, user.Id, instant, ipAddress.ToString(), client, Guid.NewGuid(), instant + refreshTokenLifeTime);

            await userAppLoginRepository.DeleteExpiredByUserIdAsync(user.Id, instant);
            login = await userAppLoginRepository.CreateAsync(login);

            var token = await tokenGenerator.GenerateAppToken(login);

            return Result.Status(OperationStatus.OK, new UserToken(token, login.RefreshToken, login.RefreshTokenExpires));
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            LogUserOutAppCommand request,
            CancellationToken cancellationToken
        )
        {
            await userAppLoginRepository.DeleteByIdAsync(request.LoginId);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus, UserToken>> HandleAsync(
            UpdateUserAppTokenCommand request,
            CancellationToken cancellationToken
        )
        {
            var app = request.App;
            var login = request.Login;

            if (login.RefreshTokenExpires < getInstant())
                return Result.Status(OperationStatus.Forbidden, default(UserToken)).Error("Refresh token expired");

            var token = await tokenGenerator.GenerateAppToken(login);

            return Result.Status(OperationStatus.OK, new UserToken(token, login.RefreshToken, login.RefreshTokenExpires));
        }
    }
}