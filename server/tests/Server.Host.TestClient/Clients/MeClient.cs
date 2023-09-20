using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Me;
using Server.ViewModels.Responses.Login;
using Server.ViewModels.Responses.Me;

namespace Server.Host.TestClient.Clients;

public class MeClient
{
    private readonly IHttpRequest _request;

    internal MeClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IHttpResponse<IResult>> RegisterMe(
        RegisterMeRequest body
    )
    {
        return await _request
            .Post("me")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult<TokensResponse>>> ConfirmMyEmail(
        Guid appId,
        ConfirmMyEmailRequestBody body
    )
    {
        return await _request
            .Post($"me/{appId}/confirm-email")
            .JsonContent(body)
            .AsResponseAsync<IResult<TokensResponse>>();
    }

    public async Task<IHttpResponse<IResult>> RestoreMyAccess(
        Guid appId,
        RestoreMyAccessRequestBody body
    )
    {
        return await _request
            .Post($"me/{appId}/restore-access")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult<MeResponse>>> GetMe(
    )
    {
        return await _request
            .Get("me")
            .AsResponseAsync<IResult<MeResponse>>();
    }

    public async Task<IHttpResponse<IResult<IdTokenResponse>>> GetMyToken(
    )
    {
        return await _request
            .Get("me/token")
            .AsResponseAsync<IResult<IdTokenResponse>>();
    }

    public async Task<IHttpResponse<IResult>> UpdateMyProfile(
        UpdateMyProfileRequest body
    )
    {
        return await _request
            .Put("me/profile")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> UpdateMyPassword(
        UpdateMyPasswordRequest body
    )
    {
        return await _request
            .Put("me/password")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> UnregisterMe(
    )
    {
        return await _request
            .Delete("me")
            .AsResponseAsync<IResult>();
    }
}