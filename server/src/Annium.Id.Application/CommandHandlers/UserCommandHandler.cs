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
    internal class UserCommandHandler : ICommandHandler<CreateUserCommand, Guid>, ICommandHandler<LoginUserCommand, UserToken>
    {
        private static readonly Duration refreshTokenLifeTime = Duration.FromDays(1);

        private readonly Func<Instant> getInstant;
        private readonly IUserRepository userRepository;
        private readonly IUserLoginRepository userLoginRepository;
        private readonly ISecurityManager securityManager;
        private readonly IIdentityDataAccessor identityDataAccessor;
        private readonly ITokenGenerator tokenGenerator;

        public UserCommandHandler(
            Func<Instant> getInstant,
            IUserRepository userRepository,
            IUserLoginRepository userLoginRepository,
            ISecurityManager securityManager,
            IIdentityDataAccessor identityDataAccessor,
            ITokenGenerator tokenGenerator
        )
        {
            this.getInstant = getInstant;
            this.userRepository = userRepository;
            this.userLoginRepository = userLoginRepository;
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

            return Result.New(OperationStatus.OK, user.Id);
        }

        public async Task<IStatusResult<OperationStatus, UserToken>> HandleAsync(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.User;

            if (securityManager.Hash(request.Password) != user.PasswordHash)
                return Result.New<OperationStatus, UserToken>(OperationStatus.Forbidden, null).Error("Invalid password");

            var instant = getInstant();
            var(ipAddress, client) = identityDataAccessor.GetIdentityData();
            var login = new UserLogin(user.Id, instant, ipAddress.ToString(), client, Guid.NewGuid(), instant + refreshTokenLifeTime);

            await userLoginRepository.DeleteExpiredByUserIdAsync(user.Id, instant);
            login = await userLoginRepository.CreateAsync(login);

            var token = tokenGenerator.GenerateBaseToken(login);
            var userToken = new UserToken(token, login.RefreshToken, login.RefreshTokenExpires);

            return Result.New<OperationStatus, UserToken>(OperationStatus.OK, userToken);
        }
    }
}