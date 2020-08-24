using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Apps;
using Annium.Id.Api.ViewModels.Responses.Apps;
using Annium.Net.Http;

namespace Annium.Id.Site.Shared.Api.Server.Clients
{
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
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<Guid>>();
        }

        public async Task<IResult<IEnumerable<AppResponse>>> FindApps(
            string query
        )
        {
            return await Request.Clone()
                .Get("apps")
                .Param("query", query)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<IEnumerable<AppResponse>>>();
        }

        public async Task<IResult<IEnumerable<AppResponse>>> ListMyApps(
        )
        {
            return await Request.Clone()
                .Get("apps/my")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<IEnumerable<AppResponse>>>();
        }

        public async Task<IResult<AppResponse>> GetApp(
            Guid appId
        )
        {
            return await Request.Clone()
                .Get($"apps/{appId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<AppResponse>>();
        }

        public async Task<IResult<Guid>> GetAppApiToken(
            Guid appId
        )
        {
            return await Request.Clone()
                .Get($"apps/{appId}/token")
                .EnsureSuccessStatusCode()
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
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> SetAppOwner(
            Guid appId,
            Guid newOwnerId
        )
        {
            return await Request.Clone()
                .Put($"apps/{appId}/owner/{newOwnerId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult<Guid>> UpdateAppApiToken(
            Guid appId
        )
        {
            return await Request.Clone()
                .Put($"apps/{appId}/token")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<Guid>>();
        }

        public async Task<IResult> DeleteApp(
            Guid appId
        )
        {
            return await Request.Clone()
                .Delete($"apps/{appId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }
    }
}