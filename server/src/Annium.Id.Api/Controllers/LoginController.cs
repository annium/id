using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.AspNetCore;
using Annium.Id.ViewModels.Login.Requests;
using Annium.Id.ViewModels.Login.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("me/{appKey}")]
    public class LoginController : ServerController
    {
        public LoginController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost("login")]
        public Task<IActionResult> LogIn(string appKey, [FromBody] LogInRequestBase requestBase)
        {
            var request = new LogInRequest
            {
                AppKey = appKey,
                Login = requestBase.Login,
                Password = requestBase.Password,
            };

            return HandleAsync<LogInRequest, TokensResponse>(request);
        }

        [HttpPut("token")]
        [Authorize("canRefreshToken", validateAudience: false, allowedExpirationInMinutes: 24 * 60)]
        public Task<IActionResult> UpdateToken(string appKey, [FromQuery] UpdateTokensRequestBase requestBase)
        {
            var request = new UpdateTokensRequest
            {
                AppKey = appKey,
                RefreshToken = requestBase.RefreshToken,
            };

            return HandleAsync<UpdateTokensRequest, TokensResponse>(request);
        }

        [HttpDelete("logout")]
        [Authorize("canLogOut", validateAudience: false, allowedExpirationInMinutes: 24 * 60)]
        public Task<IActionResult> LogOut(string appKey)
        {
            var request = new LogOutRequest { AppKey = appKey };

            return HandleAsync(request);
        }
    }
}