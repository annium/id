using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Tools;
using Annium.Id.Api.Views;
using Annium.Id.Application.Tools;
using Annium.Id.AspNetCore;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;
using Annium.Id.ViewModels.User.Requests;
using Annium.Id.ViewModels.User.Responses;
using Annium.Localization.Abstractions;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace Annium.Id.Api.Controllers
{
    [Route("new/me")]
    public class NewLoginController : ServerController
    {
        private static readonly Duration refreshTokenLifeTime = Duration.FromDays(1);
        private readonly ITokenAccessor tokenAccessor;
        private readonly IAppRepository appRepository;
        private readonly IUserRepository userRepository;
        private readonly IUserLoginRepository userLoginRepository;
        private readonly IUserAppLoginRepository userAppLoginRepository;
        private readonly IIdentityDataAccessor identityDataAccessor;
        private readonly ISecurityManager securityManager;
        private readonly ITokenGenerator tokenGenerator;
        private readonly Func<Instant> getInstant;

        public NewLoginController(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository,
            IUserRepository userRepository,
            IUserLoginRepository userLoginRepository,
            IUserAppLoginRepository userAppLoginRepository,
            IIdentityDataAccessor identityDataAccessor,
            ISecurityManager securityManager,
            ITokenGenerator tokenGenerator,
            Func<Instant> getInstant,
            IMediator mediator
        ) : base(mediator)
        {
            this.tokenAccessor = tokenAccessor;
            this.appRepository = appRepository;
            this.userRepository = userRepository;
            this.userLoginRepository = userLoginRepository;
            this.userAppLoginRepository = userAppLoginRepository;
            this.identityDataAccessor = identityDataAccessor;
            this.securityManager = securityManager;
            this.tokenGenerator = tokenGenerator;
            this.getInstant = getInstant;
        }

        [HttpPost("login")]
        public Task<IActionResult> LoginAsync([FromBody] LoginUserRequest request)
        {
            return HandleAsync<LoginUserRequest, UserTokenResponse>(request);
        }

        [HttpDelete("logout")]
        [Authorize]
        public async Task<IActionResult> LogoutAsync()
        {
            var loginId = tokenAccessor.GetBaseToken().LoginId;

            await userLoginRepository.DeleteByIdAsync(loginId);

            return NoContent();
        }

        [HttpPut("token")]
        public async Task<IActionResult> UpdateTokenAsync(Guid refreshToken)
        {
            var login = await userLoginRepository.FindByRefreshTokenAsync(refreshToken);
            if (login == null)
                return Forbidden("Invalid refresh token");

            if (login.RefreshTokenExpires < getInstant())
                return Forbidden("Refresh token expired");

            var token = tokenGenerator.GenerateBaseToken(login);

            return Ok(new UserTokenView(token, login.RefreshToken, login.RefreshTokenExpires));
        }

        [HttpPost("apps/{appId:guid}/login")]
        public async Task<IActionResult> LoginAppAsync(Guid appId, [FromBody] UserLoginPayload loginPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound("Application not found");

            var user = await userRepository.FindByLoginAsync(loginPayload.Login);
            if (user == null)
                return Forbidden("Invalid login");

            if (securityManager.Hash(loginPayload.Password) != user.PasswordHash)
                return Forbidden("Invalid password");

            var instant = getInstant();
            var(ipAddress, client) = identityDataAccessor.GetIdentityData();
            var login = new UserAppLogin(app.Id, user.Id, instant, ipAddress.ToString(), client, Guid.NewGuid(), instant + refreshTokenLifeTime);

            await userAppLoginRepository.DeleteExpiredByUserIdAsync(user.Id, instant);
            login = await userAppLoginRepository.CreateAsync(login);

            var token = await tokenGenerator.GenerateAppToken(login);

            return Ok(new UserTokenView(token, login.RefreshToken, login.RefreshTokenExpires));
        }

        [HttpDelete("apps/{appId:guid}/logout")]
        [Authorize]
        public async Task<IActionResult> LogoutAppAsync(Guid appId)
        {
            var loginId = tokenAccessor.GetBaseToken().LoginId;

            await userAppLoginRepository.DeleteByIdAsync(loginId);

            return NoContent();
        }

        [HttpPut("apps/{appId:guid}/token")]
        public async Task<IActionResult> UpdateTokenAsync(Guid appId, Guid refreshToken)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound("Application not found");

            var login = await userAppLoginRepository.FindByRefreshTokenAsync(refreshToken);
            if (login == null)
                return Forbidden("Invalid refresh token");

            if (login.RefreshTokenExpires < getInstant())
                return Forbidden("Refresh token expired");

            var token = await tokenGenerator.GenerateAppToken(login);

            return Ok(new UserTokenView(token, login.RefreshToken, login.RefreshTokenExpires));
        }
    }
}