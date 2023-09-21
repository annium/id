using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Login;
using Server.ViewModels.Responses.Login;

namespace Server.Host.TestClient.Clients;

public class LoginClient
{
    private readonly IHttpRequest _request;

    internal LoginClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IHttpResponse<IResult<TokensResponse>>> LogIn(
        Guid appId,
        LogInRequestBody body,
        IResult<TokensResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"me/{appId}/login")
            .JsonContent(body)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<TokensResponse>>> UpdateToken(
        Guid appId,
        Guid refreshToken,
        IResult<TokensResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Put($"me/{appId}/token")
            .Param("refreshToken", refreshToken)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> LogOut(
        Guid appId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Delete($"me/{appId}/logout")
            .AsResponseAsync(defaultValue, ct);
    }
}