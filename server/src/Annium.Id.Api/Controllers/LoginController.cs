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
    [Route("me/{appKey}")]
    public class LoginController : ServerController
    {
        public LoginController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost("login")]
        public Task<IActionResult> LoginAsync(string appKey, [FromBody] LogInRequest request)
        {
            request.AppKey = appKey;

            return HandleAsync<LogInRequest, TokensResponse>(request);
        }

        [HttpPut("token")]
        public Task<IActionResult> UpdateTokenAsync(string appKey, [FromQuery] UpdateTokensRequest request)
        {
            request.AppKey = appKey;

            return HandleAsync<UpdateTokensRequest, TokensResponse>(request);
        }

        [HttpDelete("logout")]
        [Authorize]
        public Task<IActionResult> LogoutAsync(string appKey)
        {
            var request = new LogOutRequest { AppKey = appKey };

            return HandleAsync(request);
        }
    }
}