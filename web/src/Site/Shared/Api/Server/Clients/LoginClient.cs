using System;
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

    public async Task<IResult<TokensResponse>> LogIn(
        Guid appId,
        LogInRequestBody body
    )
    {
        return await _request
            .Post($"me/{appId}/login")
            .JsonContent(body)
            .AsAsync<IResult<TokensResponse>>();
    }

    public async Task<IResult<TokensResponse>> UpdateToken(
        Guid appId,
        Guid refreshToken
    )
    {
        return await _request
            .Put($"me/{appId}/token")
            .Param("refreshToken", refreshToken)
            .AsAsync<IResult<TokensResponse>>();
    }

    public async Task<IResult> LogOut(
        Guid appId
    )
    {
        return await _request
            .Delete($"me/{appId}/logout")
            .AsAsync<IResult>();
    }
}