using System;
using System.Threading;
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

    public async Task<IResult> RegisterMeAsync(
        RegisterMeRequest body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post("me").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult<TokensResponse>> ConfirmMyEmailAsync(
        Guid appId,
        ConfirmMyEmailRequestBody body,
        IResult<TokensResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post($"me/{appId}/confirm-email").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult> RestoreMyAccessAsync(
        Guid appId,
        RestoreMyAccessRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post($"me/{appId}/restore-access").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult<MeResponse>> GetMeAsync(IResult<MeResponse> defaultValue, CancellationToken ct = default)
    {
        return await _request.Get("me").AsAsync(defaultValue, ct);
    }

    public async Task<IResult<IdTokenResponse>> GetMyTokenAsync(
        IResult<IdTokenResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("me/token").AsAsync(defaultValue, ct);
    }

    public async Task<IResult> UpdateMyProfileAsync(
        UpdateMyProfileRequest body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put("me/profile").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult> UpdateMyPasswordAsync(
        UpdateMyPasswordRequest body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put("me/password").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult> UnregisterMeAsync(IResult defaultValue, CancellationToken ct = default)
    {
        return await _request.Delete("me").AsAsync(defaultValue, ct);
    }
}
