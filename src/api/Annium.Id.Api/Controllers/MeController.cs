using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Me;
using Annium.Id.Api.ViewModels.Responses.Login;
using Annium.Id.Api.ViewModels.Responses.Me;
using Annium.Id.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers;

[Route("me")]
public class MeController : ServerController
{
    public MeController(
        IMediator mediator,
        IServiceProvider sp
    ) : base(mediator, sp)
    {
    }

    [HttpPost]
    public Task<IResult> RegisterMe([FromBody] RegisterMeRequest request)
    {
        return HandleAsync(request);
    }

    [HttpPost("{appId:guid}/confirm-email")]
    public Task<IResult<TokensResponse>> ConfirmMyEmail(Guid appId,
        [FromBody] ConfirmMyEmailRequestBody requestBody)
    {
        var request = new ConfirmMyEmailRequest
        {
            AppId = appId,
            Id = requestBody.Id
        };

        return HandleAsync<ConfirmMyEmailRequest, TokensResponse>(request);
    }

    [HttpPost("{appId:guid}/restore-access")]
    public Task<IResult> RestoreMyAccess(Guid appId, [FromBody] RestoreMyAccessRequestBody requestBody)
    {
        var request = new RestoreMyAccessRequest
        {
            AppId = appId,
            Server = requestBody.Server,
            Email = requestBody.Email
        };

        return HandleAsync(request);
    }

    [HttpGet]
    [Authorize(false)]
    public Task<IResult<MeResponse>> GetMe()
    {
        return HandleAsync<GetMeRequest, MeResponse>(new GetMeRequest());
    }

    [HttpGet("token")]
    [Authorize(false)]
    public Task<IResult<IdTokenResponse>> GetMyToken()
    {
        return HandleAsync<GetMyTokenRequest, IdTokenResponse>(new GetMyTokenRequest());
    }

    [HttpPut("profile")]
    [Authorize(false)]
    public Task<IResult> UpdateMyProfile([FromBody] UpdateMyProfileRequest request)
    {
        return HandleAsync(request);
    }

    [HttpPut("password")]
    [Authorize(false)]
    public Task<IResult> UpdateMyPassword([FromBody] UpdateMyPasswordRequest request)
    {
        return HandleAsync(request);
    }

    [HttpDelete]
    [Authorize(false)]
    public Task<IResult> UnregisterMe()
    {
        return HandleAsync(new UnregisterMeRequest());
    }
}