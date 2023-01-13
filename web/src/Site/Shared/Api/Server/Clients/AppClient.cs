using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Apps;
using Server.ViewModels.Responses.Apps;

namespace Site.Shared.Api.Server.Clients;

public class AppClient : ClientBase
{
    public AppClient(IHttpRequest request) : base(request)
    {
    }

    public async Task<IResult<Guid>> CreateApp(
        CreateAppRequest body
    )
    {
        return await Request.Clone()
            .Post("apps")
            .JsonContent(body)
            .AsAsync(Result.New(default(Guid)).Error("Request failed"));
    }

    public async Task<IResult<IEnumerable<AppResponse>>> FindApps(
        string query
    )
    {
        return await Request.Clone()
            .Get("apps")
            .Param("query", query)
            .AsAsync(Result.New<IEnumerable<AppResponse>>(Array.Empty<AppResponse>()).Error("Request failed"));
    }

    public async Task<IResult<IEnumerable<AppResponse>>> ListMyApps(
    )
    {
        return await Request.Clone()
            .Get("apps/my")
            .AsAsync(Result.New<IEnumerable<AppResponse>>(Array.Empty<AppResponse>()).Error("Request failed"));
    }

    public async Task<IResult<AppResponse>> GetApp(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get($"apps/{appId}")
            .AsAsync(Result.New(new AppResponse()).Error("Request failed"));
    }

    public async Task<IResult<Guid>> GetAppApiToken(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get($"apps/{appId}/token")
            .AsAsync(Result.New(default(Guid)).Error("Request failed"));
    }

    public async Task<IResult> UpdateApp(
        Guid appId,
        UpdateAppRequestBody body
    )
    {
        return await Request.Clone()
            .Put($"apps/{appId}")
            .JsonContent(body)
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult> SetAppOwner(
        Guid appId,
        Guid newOwnerId
    )
    {
        return await Request.Clone()
            .Put($"apps/{appId}/owner/{newOwnerId}")
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult<Guid>> UpdateAppApiToken(
        Guid appId
    )
    {
        return await Request.Clone()
            .Put($"apps/{appId}/token")
            .AsAsync(Result.New(default(Guid)).Error("Request failed"));
    }

    public async Task<IResult> DeleteApp(
        Guid appId
    )
    {
        return await Request.Clone()
            .Delete($"apps/{appId}")
            .AsAsync(Result.New().Error("Request failed"));
    }
}