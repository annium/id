using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Login;
using Server.ViewModels.Responses.Login;

namespace Site.Shared.Api.Server.Clients;

public class LoginClient
{
    private readonly IHttpRequest _request;

    internal LoginClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IResult<TokensResponse>> LogInAsync(
        Guid appId,
        LogInRequestBody body,
        IResult<TokensResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post($"me/{appId}/login").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult<TokensResponse>> UpdateTokenAsync(
        Guid appId,
        Guid refreshToken,
        IResult<TokensResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"me/{appId}/token").Param("refreshToken", refreshToken).AsAsync(defaultValue, ct);
    }

    public async Task<IResult> LogOutAsync(Guid appId, IResult defaultValue, CancellationToken ct = default)
    {
        return await _request.Delete($"me/{appId}/logout").AsAsync(defaultValue, ct);
    }
}
