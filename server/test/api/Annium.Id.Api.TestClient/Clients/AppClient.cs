using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Apps.Requests;
using Annium.Id.Api.ViewModels.Apps.Responses;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient.Clients
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

        public async Task<IHttpResponse<IResult<IEnumerable<AppResponse>>>> ListApps(
        )
        {
            return await Request.Clone()
                .Get("apps")
                .AsResponseAsync<IResult<IEnumerable<AppResponse>>>();
        }

        public async Task<IHttpResponse<IResult<AppResponse>>> GetApp(
            Guid appId
        )
        {
            return await Request.Clone()
                .Get($"apps/{appId:guid}")
                .AsResponseAsync<IResult<AppResponse>>();
        }

        public async Task<IHttpResponse<IResult<Guid>>> GetAppApiToken(
            Guid appId
        )
        {
            return await Request.Clone()
                .Get($"apps/{appId:guid}/token")
                .AsResponseAsync<IResult<Guid>>();
        }

        public async Task<IHttpResponse<IResult>> UpdateApp(
            Guid appId,
            UpdateAppRequestBase body
        )
        {
            return await Request.Clone()
                .Put($"apps/{appId:guid}")
                .JsonContent(body)
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> SetAppOwner(
            Guid appId,
            Guid newOwnerId
        )
        {
            return await Request.Clone()
                .Put($"apps/{appId:guid}/owner/{newOwnerId:guid}")
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult<Guid>>> UpdateAppApiToken(
            Guid appId
        )
        {
            return await Request.Clone()
                .Put($"apps/{appId:guid}/token")
                .AsResponseAsync<IResult<Guid>>();
        }

        public async Task<IHttpResponse<IResult>> DeleteApp(
            Guid appId
        )
        {
            return await Request.Clone()
                .Delete($"apps/{appId:guid}")
                .AsResponseAsync<IResult>();
        }
    }
}