using System;
using System.Threading.Tasks;
using Annium.Id.Application.Tools;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;
using NodaTime;

namespace Annium.Id.Application.Services
{
    internal class LoginService : ILoginService
    {
        private static readonly Duration refreshTokenLifeTime = Duration.FromDays(1);
        private readonly Func<Instant> getInstant;
        private readonly IUserLoginRepository userLoginRepository;
        private readonly IIdentityDataAccessor identityDataAccessor;
        private readonly ITokenGenerator tokenGenerator;

        public LoginService(
            Func<Instant> getInstant,
            IUserLoginRepository userLoginRepository,
            IIdentityDataAccessor identityDataAccessor,
            ITokenGenerator tokenGenerator
        )
        {
            this.getInstant = getInstant;
            this.userLoginRepository = userLoginRepository;
            this.identityDataAccessor = identityDataAccessor;
            this.tokenGenerator = tokenGenerator;
        }

        public async Task<Tokens> LogUserInAsync(App app, User user)
        {
            var instant = getInstant();
            var (ipAddress, client) = identityDataAccessor.GetIdentityData();
            var login = new UserLogin(app.Id, user.Id, instant, ipAddress.ToString(), client, Guid.NewGuid(), instant + refreshTokenLifeTime);

            await userLoginRepository.DeleteExpiredByUserIdAsync(user.Id, instant);
            login = await userLoginRepository.CreateAsync(login);

            var token = await tokenGenerator.GenerateToken(login);

            return new Tokens(token, login.RefreshToken, login.RefreshTokenExpires);
        }
    }
}