using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Login;
using Annium.Id.Api.ViewModels.Responses.Login;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient
{
    public class LoginClient : ClientBase
    {
        public LoginClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IHttpResponse<IResult<TokensResponse>>> LogIn(
            Guid appId,
            LogInRequestBody body
        )
        {
            return await Request.Clone()
                .Post($"me/{appId}/login")
                .JsonContent(body)
                .AsResponseAsync<IResult<TokensResponse>>();
        }

        public async Task<IHttpResponse<IResult<TokensResponse>>> UpdateToken(
            Guid appId,
            Guid refreshToken
        )
        {
            return await Request.Clone()
                .Put($"me/{appId}/token")
                .Param("refreshToken", refreshToken)
                .AsResponseAsync<IResult<TokensResponse>>();
        }

        public async Task<IHttpResponse<IResult>> LogOut(
            Guid appId
        )
        {
            return await Request.Clone()
                .Delete($"me/{appId}/logout")
                .AsResponseAsync<IResult>();
        }
    }
}