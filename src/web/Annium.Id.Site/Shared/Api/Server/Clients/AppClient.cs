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

        public async Task<IHttpResponse<IResult<Guid>>> CreateApp(
            CreateAppRequest body
        )
        {
            return await Request.Clone()
                .Post("apps")
                .JsonContent(body)
                .AsResponseAsync<IResult<Guid>>();
        }

        public async Task<IHttpResponse<IResult<IEnumerable<AppResponse>>>> FindApps(
            string query
        )
        {
            return await Request.Clone()
                .Get("apps")
                .Param("query", query)
                .AsResponseAsync<IResult<IEnumerable<AppResponse>>>();
        }

        public async Task<IHttpResponse<IResult<IEnumerable<AppResponse>>>> ListMyApps(
        )
        {
            return await Request.Clone()
                .Get("apps/my")
                .AsResponseAsync<IResult<IEnumerable<AppResponse>>>();
        }

        public async Task<IHttpResponse<IResult<AppResponse>>> GetApp(
            Guid appId
        )
        {
            return await Request.Clone()
                .Get($"apps/{appId}")
                .AsResponseAsync<IResult<AppResponse>>();
        }

        public async Task<IHttpResponse<IResult<Guid>>> GetAppApiToken(
            Guid appId
        )
        {
            return await Request.Clone()
                .Get($"apps/{appId}/token")
                .AsResponseAsync<IResult<Guid>>();
        }

        public async Task<IHttpResponse<IResult>> UpdateApp(
            Guid appId,
            UpdateAppRequestBody body
        )
        {
            return await Request.Clone()
                .Put($"apps/{appId}")
                .JsonContent(body)
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> SetAppOwner(
            Guid appId,
            Guid newOwnerId
        )
        {
            return await Request.Clone()
                .Put($"apps/{appId}/owner/{newOwnerId}")
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult<Guid>>> UpdateAppApiToken(
            Guid appId
        )
        {
            return await Request.Clone()
                .Put($"apps/{appId}/token")
                .AsResponseAsync<IResult<Guid>>();
        }

        public async Task<IHttpResponse<IResult>> DeleteApp(
            Guid appId
        )
        {
            return await Request.Clone()
                .Delete($"apps/{appId}")
                .AsResponseAsync<IResult>();
        }
    }
}