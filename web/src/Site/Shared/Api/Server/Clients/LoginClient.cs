using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Login;
using Server.ViewModels.Responses.Login;

namespace Site.Shared.Api.Server.Clients;

public class LoginClient : ClientBase
{
    public LoginClient(IHttpRequest request) : base(request)
    {
    }

    public async Task<IResult<TokensResponse>> LogIn(
        Guid appId,
        LogInRequestBody body
    )
    {
        return await Request.Clone()
            .Post($"me/{appId}/login")
            .JsonContent(body)
            .AsAsync(Result.New(new TokensResponse()).Error("Request failed"));
    }

    public async Task<IResult<TokensResponse>> UpdateToken(
        Guid appId,
        Guid refreshToken
    )
    {
        return await Request.Clone()
            .Put($"me/{appId}/token")
            .Param("refreshToken", refreshToken)
            .AsAsync(Result.New(new TokensResponse()).Error("Request failed"));
    }

    public async Task<IResult> LogOut(
        Guid appId
    )
    {
        return await Request.Clone()
            .Delete($"me/{appId}/logout")
            .AsAsync(Result.New().Error("Request failed"));
    }
}