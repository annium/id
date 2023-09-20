using System;
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
        LogInRequestBody body
    )
    {
        return await _request
            .Post($"me/{appId}/login")
            .JsonContent(body)
            .AsResponseAsync<IResult<TokensResponse>>();
    }

    public async Task<IHttpResponse<IResult<TokensResponse>>> UpdateToken(
        Guid appId,
        Guid refreshToken
    )
    {
        return await _request
            .Put($"me/{appId}/token")
            .Param("refreshToken", refreshToken)
            .AsResponseAsync<IResult<TokensResponse>>();
    }

    public async Task<IHttpResponse<IResult>> LogOut(
        Guid appId
    )
    {
        return await _request
            .Delete($"me/{appId}/logout")
            .AsResponseAsync<IResult>();
    }
}