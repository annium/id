using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Login;
using Annium.Id.Api.ViewModels.Responses.Login;
using Annium.Id.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("me/{appId:guid}")]
    public class LoginController : ServerController
    {
        public LoginController(
            IMediator mediator
        ) : base(mediator)
        {
        }

        [HttpPost("login")]
        public Task<IResult<TokensResponse>> LogIn(Guid appId, [FromBody] LogInRequestBody requestBody)
        {
            var request = new LogInRequest
            {
                AppId = appId,
                Login = requestBody.Login,
                Password = requestBody.Password
            };

            return HandleAsync<LogInRequest, TokensResponse>(request);
        }

        [HttpPut("token")]
        [Authorize(AuthPolicy.CanRefreshToken, false, false)]
        public Task<IResult<TokensResponse>> UpdateToken(Guid appId, [FromQuery] UpdateTokensRequestBody requestBody)
        {
            var request = new UpdateTokensRequest
            {
                AppId = appId,
                RefreshToken = requestBody.RefreshToken
            };

            return HandleAsync<UpdateTokensRequest, TokensResponse>(request);
        }

        [HttpDelete("logout")]
        [Authorize(AuthPolicy.CanLogOut, false)]
        public Task<IResult> LogOut(Guid appId)
        {
            var request = new LogOutRequest { AppId = appId };

            return HandleAsync(request);
        }
    }
}