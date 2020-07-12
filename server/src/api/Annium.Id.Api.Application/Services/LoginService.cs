using System;
using System.Threading.Tasks;
using Annium.Id.Api.Application.Tools;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;
using NodaTime;

namespace Annium.Id.Api.Application.Services
{
    internal class LoginService : ILoginService
    {
        private readonly Func<Instant> getInstant;
        private readonly AuthOptions options;
        private readonly IUserLoginRepository userLoginRepository;
        private readonly IIdentityDataAccessor identityDataAccessor;
        private readonly ITokenGenerator tokenGenerator;

        public LoginService(
            Func<Instant> getInstant,
            AuthOptions options,
            IUserLoginRepository userLoginRepository,
            IIdentityDataAccessor identityDataAccessor,
            ITokenGenerator tokenGenerator
        )
        {
            this.getInstant = getInstant;
            this.options = options;
            this.userLoginRepository = userLoginRepository;
            this.identityDataAccessor = identityDataAccessor;
            this.tokenGenerator = tokenGenerator;
        }

        public async Task<Tokens> LogUserInAsync(App app, User user)
        {
            var instant = getInstant();
            var identity = identityDataAccessor.GetIdentityData();
            var login = new UserLogin(
                app.Id,
                user.Id,
                instant,
                identity.IPAddress.ToString(),
                identity.Client,
                Guid.NewGuid(),
                instant + options.RefreshTokenLifeTime
            );

            await userLoginRepository.DeleteExpiredByUserIdAsync(user.Id, instant);
            login = await userLoginRepository.CreateAsync(login);

            var token = await tokenGenerator.GenerateTokenString(login);

            return new Tokens(token, login.RefreshToken, login.RefreshTokenExpires);
        }
    }
}