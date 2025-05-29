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

    public async Task<IHttpResponse<IResult>> RegisterMeAsync(
        RegisterMeRequest body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post("me").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<TokensResponse>>> ConfirmMyEmailAsync(
        Guid appId,
        ConfirmMyEmailRequestBody body,
        IResult<TokensResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post($"me/{appId}/confirm-email").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> RestoreMyAccessAsync(
        Guid appId,
        RestoreMyAccessRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post($"me/{appId}/restore-access").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<MeResponse>>> GetMeAsync(
        IResult<MeResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("me").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<IdTokenResponse>>> GetMyTokenAsync(
        IResult<IdTokenResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("me/token").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> UpdateMyProfileAsync(
        UpdateMyProfileRequest body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put("me/profile").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> UpdateMyPasswordAsync(
        UpdateMyPasswordRequest body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put("me/password").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> UnregisterMeAsync(IResult defaultValue, CancellationToken ct = default)
    {
        return await _request.Delete("me").AsResponseAsync(defaultValue, ct);
    }
}
