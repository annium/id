using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Tools;
using Annium.Id.Api.Views;
using Annium.Id.AspNetCore;
using Annium.Id.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NodaTime;

namespace Annium.Id.Api.Controllers
{
    [Route("me")]
    public class MeController : LocalizedServerController
    {
        private static readonly Duration refreshTokenLifeTime = Duration.FromDays(1);

        private readonly IUserRepository userRepository;

        private readonly IUserLoginRepository userLoginRepository;

        private readonly IIdentityDataAccessor identityDataAccessor;

        private readonly ISecurityManager securityManager;

        private readonly ITokenGenerator tokenGenerator;

        private readonly Func<Instant> getInstant;

        public MeController(
            IUserRepository userRepository,
            IUserLoginRepository userLoginRepository,
            IIdentityDataAccessor identityDataAccessor,
            ISecurityManager securityManager,
            ITokenGenerator tokenGenerator,
            Func<Instant> getInstant,
            IStringLocalizer<MeController> localizer
        ) : base(localizer)
        {
            this.userRepository = userRepository;
            this.userLoginRepository = userLoginRepository;
            this.identityDataAccessor = identityDataAccessor;
            this.securityManager = securityManager;
            this.tokenGenerator = tokenGenerator;
            this.getInstant = getInstant;
        }

        [HttpPut]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserPayload userPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if ((await userRepository.FindByLoginAsync(userPayload.Login)) != null)
                return Conflict($"Login {userPayload.Login} is already used");

            if ((await userRepository.FindByEmailAsync(userPayload.Email)) != null)
                return Conflict($"Email {userPayload.Email} is already used");

            var passwordHash = securityManager.Hash(userPayload.Password);

            var user = new User(
                userPayload.Login,
                passwordHash,
                userPayload.FirstName,
                userPayload.LastName,
                userPayload.Email
            );

            user = await userRepository.CreateAsync(user);

            return Ok(user);
        }

        [HttpGet]
        [AuthorizeId]
        public async Task<IActionResult> GetUserAsync()
        {
            var user = await userRepository.GetByIdAsync(this.GetId().UserId);
            if (user == null)
                return NotFound();

            // TODO: perhaps, add info about organizations, user is member of
            return Ok(new UserView(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginPayload loginPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userRepository.FindByLoginAsync(loginPayload.Login);
            if (user == null)
                return Forbidden("Invalid login");

            if (securityManager.Hash(loginPayload.Password) != user.PasswordHash)
                return Forbidden("Invalid password");

            var instant = getInstant();
            var(ipAddress, client) = identityDataAccessor.GetIdentityData();
            var login = new UserLogin(user.Id, instant, ipAddress.ToString(), client, Guid.NewGuid(), instant + refreshTokenLifeTime);

            login = await userLoginRepository.CreateAsync(login);

            var token = tokenGenerator.Generate(login);

            return Ok(new UserTokenView(token, login));
        }

        [HttpPost("logout")]
        [AuthorizeId]
        public async Task<IActionResult> LogoutAsync()
        {
            var loginId = this.GetId().LoginId;

            await userLoginRepository.DeleteByIdAsync(loginId);

            return NoContent();
        }

        [HttpPost("token")]
        public async Task<IActionResult> UpdateTokenAsync(Guid refreshToken)
        {
            var login = await userLoginRepository.FindByRefreshTokenAsync(refreshToken);
            if (login == null)
                return Forbidden("Invalid refresh token");

            login.RefreshToken = Guid.NewGuid();
            login.RefreshTokenExpires = getInstant();

            login = await userLoginRepository.UpdateRefreshTokenAsync(login);

            var token = tokenGenerator.Generate(login);

            return Ok(new UserTokenView(token, login));
        }

        [HttpPost]
        [AuthorizeId]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserPayload userPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userRepository.GetByIdAsync(this.GetId().UserId);

            if (userPayload.Login != user.Login && (await userRepository.FindByLoginAsync(userPayload.Login)) != null)
                return Conflict($"Login {userPayload.Login} is already used");

            if (userPayload.Email != user.Email && (await userRepository.FindByEmailAsync(userPayload.Email)) != null)
                return Conflict($"Email {userPayload.Email} is already used");

            user.Login = userPayload.Login;
            user.PasswordHash = securityManager.Hash(userPayload.Password);
            user.FirstName = userPayload.FirstName;
            user.LastName = userPayload.LastName;
            user.Email = userPayload.Email;

            user = await userRepository.UpdateAsync(user);

            return Ok(new UserView(user));
        }

        [HttpDelete]
        [AuthorizeId]
        public async Task<IActionResult> UnregisterUserAsync()
        {
            var userId = this.GetId().UserId;

            await userLoginRepository.DeleteAllByUserIdAsync(userId);
            await userRepository.DeleteByIdAsync(userId);

            return NoContent();
        }
    }
}