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

    public async Task<IResult> RegisterMe(
        RegisterMeRequest body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post("me")
            .JsonContent(body)
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult<TokensResponse>> ConfirmMyEmail(
        Guid appId,
        ConfirmMyEmailRequestBody body,
        IResult<TokensResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"me/{appId}/confirm-email")
            .JsonContent(body)
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> RestoreMyAccess(
        Guid appId,
        RestoreMyAccessRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"me/{appId}/restore-access")
            .JsonContent(body)
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult<MeResponse>> GetMe(
        IResult<MeResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Get("me")
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult<IdTokenResponse>> GetMyToken(
        IResult<IdTokenResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Get("me/token")
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> UpdateMyProfile(
        UpdateMyProfileRequest body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Put("me/profile")
            .JsonContent(body)
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> UpdateMyPassword(
        UpdateMyPasswordRequest body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Put("me/password")
            .JsonContent(body)
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> UnregisterMe(
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Delete("me")
            .AsAsync(defaultValue, ct);
    }
}