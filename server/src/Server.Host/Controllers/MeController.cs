using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Server.ViewModels.Requests.Me;
using Server.ViewModels.Responses.Login;
using Server.ViewModels.Responses.Me;

namespace Server.Host.Controllers;

[Route("me")]
public class MeController : ServerController
{
    public MeController(IMediator mediator, IServiceProvider sp)
        : base(mediator, sp) { }

    [HttpPost]
    public Task<IResult> RegisterMeAsync([FromBody] RegisterMeRequest request)
    {
        return HandleAsync(request);
    }

    [HttpPost("{appId:guid}/confirm-email")]
    public Task<IResult<TokensResponse>> ConfirmMyEmailAsync(
        Guid appId,
        [FromBody] ConfirmMyEmailRequestBody requestBody
    )
    {
        var request = new ConfirmMyEmailRequest { AppId = appId, Id = requestBody.Id };

        return HandleAsync<ConfirmMyEmailRequest, TokensResponse>(request);
    }

    [HttpPost("{appId:guid}/restore-access")]
    public Task<IResult> RestoreMyAccessAsync(Guid appId, [FromBody] RestoreMyAccessRequestBody requestBody)
    {
        var request = new RestoreMyAccessRequest
        {
            AppId = appId,
            Server = requestBody.Server,
            Email = requestBody.Email,
        };

        return HandleAsync(request);
    }

    [HttpGet]
    [Authorize(false)]
    public Task<IResult<MeResponse>> GetMeAsync()
    {
        return HandleAsync<GetMeRequest, MeResponse>(new GetMeRequest());
    }

    [HttpGet("token")]
    [Authorize(false)]
    public Task<IResult<IdTokenResponse>> GetMyTokenAsync()
    {
        return HandleAsync<GetMyTokenRequest, IdTokenResponse>(new GetMyTokenRequest());
    }

    [HttpPut("profile")]
    [Authorize(false)]
    public Task<IResult> UpdateMyProfileAsync([FromBody] UpdateMyProfileRequest request)
    {
        return HandleAsync(request);
    }

    [HttpPut("password")]
    [Authorize(false)]
    public Task<IResult> UpdateMyPasswordAsync([FromBody] UpdateMyPasswordRequest request)
    {
        return HandleAsync(request);
    }

    [HttpDelete]
    [Authorize(false)]
    public Task<IResult> UnregisterMeAsync()
    {
        return HandleAsync(new UnregisterMeRequest());
    }
}
