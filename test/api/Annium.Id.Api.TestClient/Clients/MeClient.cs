using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Me;
using Annium.Id.Api.ViewModels.Responses.Login;
using Annium.Id.Api.ViewModels.Responses.Me;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient.Clients
{
    public class MeClient : ClientBase
    {
        public MeClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IHttpResponse<IResult>> RegisterMe(
            RegisterMeRequest body
        )
        {
            return await Request.Clone()
                .Post("me")
                .JsonContent(body)
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult<TokensResponse>>> ConfirmMyEmail(
            Guid appId,
            ConfirmMyEmailRequestBody body
        )
        {
            return await Request.Clone()
                .Post($"me/{appId}/confirm-email")
                .JsonContent(body)
                .AsResponseAsync<IResult<TokensResponse>>();
        }

        public async Task<IHttpResponse<IResult>> RestoreMyAccess(
            Guid appId,
            RestoreMyAccessRequestBody body
        )
        {
            return await Request.Clone()
                .Post($"me/{appId}/restore-access")
                .JsonContent(body)
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult<MeResponse>>> GetMe(
        )
        {
            return await Request.Clone()
                .Get("me")
                .AsResponseAsync<IResult<MeResponse>>();
        }

        public async Task<IHttpResponse<IResult<IdTokenResponse>>> GetMyToken(
        )
        {
            return await Request.Clone()
                .Get("me/token")
                .AsResponseAsync<IResult<IdTokenResponse>>();
        }

        public async Task<IHttpResponse<IResult>> UpdateMyProfile(
            UpdateMyProfileRequest body
        )
        {
            return await Request.Clone()
                .Put("me/profile")
                .JsonContent(body)
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> UpdateMyPassword(
            UpdateMyPasswordRequest body
        )
        {
            return await Request.Clone()
                .Put("me/password")
                .JsonContent(body)
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> UnregisterMe(
        )
        {
            return await Request.Clone()
                .Delete("me")
                .AsResponseAsync<IResult>();
        }
    }
}