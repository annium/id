using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.AspNetCore;
using Annium.Id.ViewModels.Login.Requests;
using Annium.Id.ViewModels.Login.Responses;
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
        public Task<IActionResult> LoginAsync([FromBody] LogInRequest request)
        {
            // TODO: perhaps, add info about companies, user is member of
            return HandleAsync<LogInRequest, TokensResponse>(request);
        }

        [HttpDelete("logout")]
        [Authorize]
        public Task<IActionResult> LogoutAsync()
        {
            return HandleAsync(new LogOutRequest());
        }

        [HttpPut("token")]
        public Task<IActionResult> UpdateTokenAsync([FromQuery] UpdateTokensRequest request)
        {
            return HandleAsync<UpdateTokensRequest, TokensResponse>(request);
        }

        [HttpPost("apps/{appId:guid}/login")]
        public Task<IActionResult> LoginAppAsync(Guid appId, [FromBody] LogInAppRequest request)
        {
            request.AppId = appId;

            return HandleAsync<LogInAppRequest, TokensResponse>(request);
        }

        [HttpDelete("apps/{appId:guid}/logout")]
        [Authorize]
        public Task<IActionResult> LogoutAppAsync(Guid appId)
        {
            var request = new LogOutAppRequest { AppId = appId };

            return HandleAsync(request);
        }

        [HttpPut("apps/{appId:guid}/token")]
        public Task<IActionResult> UpdateTokenAsync(Guid appId, [FromQuery] UpdateAppTokenRequest request)
        {
            request.AppId = appId;

            return HandleAsync<UpdateAppTokenRequest, TokensResponse>(request);
        }
    }
}