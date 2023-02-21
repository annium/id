using System;
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
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult<AppResponse[]>> FindApps(
        string query
    )
    {
        return await Request.Clone()
            .Get("apps")
            .Param("query", query)
            .AsAsync<IResult<AppResponse[]>>();
    }

    public async Task<IResult<AppResponse[]>> ListMyApps(
    )
    {
        return await Request.Clone()
            .Get("apps/my")
            .AsAsync<IResult<AppResponse[]>>();
    }

    public async Task<IResult<AppResponse>> GetApp(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get($"apps/{appId}")
            .AsAsync<IResult<AppResponse>>();
    }

    public async Task<IResult<Guid>> GetAppApiToken(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get($"apps/{appId}/token")
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult> UpdateApp(
        Guid appId,
        UpdateAppRequestBody body
    )
    {
        return await Request.Clone()
            .Put($"apps/{appId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> SetAppOwner(
        Guid appId,
        Guid newOwnerId
    )
    {
        return await Request.Clone()
            .Put($"apps/{appId}/owner/{newOwnerId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult<Guid>> UpdateAppApiToken(
        Guid appId
    )
    {
        return await Request.Clone()
            .Put($"apps/{appId}/token")
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult> DeleteApp(
        Guid appId
    )
    {
        return await Request.Clone()
            .Delete($"apps/{appId}")
            .AsAsync<IResult>();
    }
}