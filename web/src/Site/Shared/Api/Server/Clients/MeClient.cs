using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Me;
using Server.ViewModels.Responses.Login;
using Server.ViewModels.Responses.Me;

namespace Site.Shared.Api.Server.Clients;

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
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult<TokensResponse>> ConfirmMyEmail(
        Guid appId,
        ConfirmMyEmailRequestBody body
    )
    {
        return await Request.Clone()
            .Post($"me/{appId}/confirm-email")
            .JsonContent(body)
            .AsAsync(Result.New(new TokensResponse()).Error("Request failed"));
    }

    public async Task<IResult> RestoreMyAccess(
        Guid appId,
        RestoreMyAccessRequestBody body
    )
    {
        return await Request.Clone()
            .Post($"me/{appId}/restore-access")
            .JsonContent(body)
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult<MeResponse>> GetMe(
    )
    {
        return await Request.Clone()
            .Get("me")
            .AsAsync(Result.New(new MeResponse()).Error("Request failed"));
    }

    public async Task<IResult<IdTokenResponse>> GetMyToken(
    )
    {
        return await Request.Clone()
            .Get("me/token")
            .AsAsync(Result.New(new IdTokenResponse()).Error("Request failed"));
    }

    public async Task<IResult> UpdateMyProfile(
        UpdateMyProfileRequest body
    )
    {
        return await Request.Clone()
            .Put("me/profile")
            .JsonContent(body)
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult> UpdateMyPassword(
        UpdateMyPasswordRequest body
    )
    {
        return await Request.Clone()
            .Put("me/password")
            .JsonContent(body)
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult> UnregisterMe(
    )
    {
        return await Request.Clone()
            .Delete("me")
            .AsAsync(Result.New().Error("Request failed"));
    }
}