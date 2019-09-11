using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.AspNetCore;
using Annium.Id.ViewModels.Users.Requests;
using Annium.Id.ViewModels.Users.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("me")]
    public class LoginController : ServerController
    {
        public LoginController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost("login")]
        public Task<IActionResult> LoginAsync([FromBody] LogUserInRequest request)
        {
            // TODO: perhaps, add info about companies, user is member of
            return HandleAsync<LogUserInRequest, UserTokenResponse>(request);
        }

        [HttpDelete("logout")]
        [Authorize]
        public Task<IActionResult> LogoutAsync()
        {
            return HandleAsync(new LogUserOutRequest());
        }

        [HttpPut("token")]
        public Task<IActionResult> UpdateTokenAsync([FromQuery] UpdateUserTokenRequest request)
        {
            return HandleAsync<UpdateUserTokenRequest, UserTokenResponse>(request);
        }

        [HttpPost("apps/{appId:guid}/login")]
        public Task<IActionResult> LoginAppAsync(Guid appId, [FromBody] LogUserInAppRequest request)
        {
            request.AppId = appId;

            return HandleAsync<LogUserInAppRequest, UserTokenResponse>(request);
        }

        [HttpDelete("apps/{appId:guid}/logout")]
        [Authorize]
        public Task<IActionResult> LogoutAppAsync(Guid appId)
        {
            var request = new LogUserOutAppRequest { AppId = appId };

            return HandleAsync(request);
        }

        [HttpPut("apps/{appId:guid}/token")]
        public Task<IActionResult> UpdateTokenAsync(Guid appId, [FromQuery] UpdateUserAppTokenRequest request)
        {
            request.AppId = appId;

            return HandleAsync<UpdateUserAppTokenRequest, UserTokenResponse>(request);
        }
    }
}