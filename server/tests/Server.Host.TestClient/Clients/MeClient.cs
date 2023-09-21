using System;
using System.Threading;
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
        RegisterMeRequest body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post("me")
            .JsonContent(body)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<TokensResponse>>> ConfirmMyEmail(
        Guid appId,
        ConfirmMyEmailRequestBody body,
        IResult<TokensResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"me/{appId}/confirm-email")
            .JsonContent(body)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> RestoreMyAccess(
        Guid appId,
        RestoreMyAccessRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"me/{appId}/restore-access")
            .JsonContent(body)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<MeResponse>>> GetMe(
        IResult<MeResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Get("me")
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<IdTokenResponse>>> GetMyToken(
        IResult<IdTokenResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Get("me/token")
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> UpdateMyProfile(
        UpdateMyProfileRequest body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Put("me/profile")
            .JsonContent(body)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> UpdateMyPassword(
        UpdateMyPasswordRequest body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Put("me/password")
            .JsonContent(body)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> UnregisterMe(
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Delete("me")
            .AsResponseAsync(defaultValue, ct);
    }
}