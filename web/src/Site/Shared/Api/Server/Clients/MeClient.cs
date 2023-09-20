using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Me;
using Server.ViewModels.Responses.Login;
using Server.ViewModels.Responses.Me;

namespace Site.Shared.Api.Server.Clients;

public class MeClient
{
    private readonly IHttpRequest _request;

    internal MeClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IResult> RegisterMe(
        RegisterMeRequest body
    )
    {
        return await _request
            .Post("me")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult<TokensResponse>> ConfirmMyEmail(
        Guid appId,
        ConfirmMyEmailRequestBody body
    )
    {
        return await _request
            .Post($"me/{appId}/confirm-email")
            .JsonContent(body)
            .AsAsync<IResult<TokensResponse>>();
    }

    public async Task<IResult> RestoreMyAccess(
        Guid appId,
        RestoreMyAccessRequestBody body
    )
    {
        return await _request
            .Post($"me/{appId}/restore-access")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult<MeResponse>> GetMe(
    )
    {
        return await _request
            .Get("me")
            .AsAsync<IResult<MeResponse>>();
    }

    public async Task<IResult<IdTokenResponse>> GetMyToken(
    )
    {
        return await _request
            .Get("me/token")
            .AsAsync<IResult<IdTokenResponse>>();
    }

    public async Task<IResult> UpdateMyProfile(
        UpdateMyProfileRequest body
    )
    {
        return await _request
            .Put("me/profile")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> UpdateMyPassword(
        UpdateMyPasswordRequest body
    )
    {
        return await _request
            .Put("me/password")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> UnregisterMe(
    )
    {
        return await _request
            .Delete("me")
            .AsAsync<IResult>();
    }
}