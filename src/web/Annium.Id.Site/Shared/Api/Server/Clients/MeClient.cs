using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Me;
using Annium.Id.Api.ViewModels.Responses.Login;
using Annium.Id.Api.ViewModels.Responses.Me;
using Annium.Net.Http;

namespace Annium.Id.Site.Shared.Api.Server.Clients
{
    public class MeClient : ClientBase
    {
        public MeClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IResult> RegisterMe(
            RegisterMeRequest body
        )
        {
            return await Request.Clone()
                .Post("me")
                .JsonContent(body)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult<TokensResponse>> ConfirmMyEmail(
            Guid appId,
            ConfirmMyEmailRequestBody body
        )
        {
            return await Request.Clone()
                .Post($"me/{appId}/confirm-email")
                .JsonContent(body)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<TokensResponse>>();
        }

        public async Task<IResult> RestoreMyAccess(
            Guid appId,
            RestoreMyAccessRequestBody body
        )
        {
            return await Request.Clone()
                .Post($"me/{appId}/restore-access")
                .JsonContent(body)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult<MeResponse>> GetMe(
        )
        {
            return await Request.Clone()
                .Get("me")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<MeResponse>>();
        }

        public async Task<IResult<IdTokenResponse>> GetMyToken(
        )
        {
            return await Request.Clone()
                .Get("me/token")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<IdTokenResponse>>();
        }

        public async Task<IResult> UpdateMyProfile(
            UpdateMyProfileRequest body
        )
        {
            return await Request.Clone()
                .Put("me/profile")
                .JsonContent(body)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> UpdateMyPassword(
            UpdateMyPasswordRequest body
        )
        {
            return await Request.Clone()
                .Put("me/password")
                .JsonContent(body)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> UnregisterMe(
        )
        {
            return await Request.Clone()
                .Delete("me")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }
    }
}