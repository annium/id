using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.AspNetCore;
using Annium.Id.Api.ViewModels.Requests.Apps;
using Annium.Id.Api.ViewModels.Responses.Apps;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("apps")]
    public class AppController : ServerController
    {
        public AppController(
            IMediator mediator
        ) : base(mediator)
        {
        }

        [HttpPost]
        [Authorize]
        public Task<IResult<Guid>> CreateApp([FromBody] CreateAppRequest request)
        {
            return HandleAsync<CreateAppRequest, Guid>(request);
        }

        [HttpGet]
        public Task<IResult<IEnumerable<AppResponse>>> ListApps()
        {
            return HandleAsync<ListAppsRequest, IEnumerable<AppResponse>>(new ListAppsRequest());
        }

        [HttpGet("{appId:guid}")]
        public Task<IResult<AppResponse>> GetApp(Guid appId)
        {
            var request = new GetAppRequest { AppId = appId };

            return HandleAsync<GetAppRequest, AppResponse>(request);
        }

        [HttpGet("{appId:guid}/token")]
        [Authorize]
        public Task<IResult<Guid>> GetAppApiToken(Guid appId)
        {
            var request = new GetAppApiTokenRequest { AppId = appId };

            return HandleAsync<GetAppApiTokenRequest, Guid>(request);
        }

        [HttpPut("{appId:guid}")]
        [Authorize]
        public Task<IResult> UpdateApp(Guid appId, [FromBody] UpdateAppRequestBody requestBody)
        {
            var request = new UpdateAppRequest
            {
                AppId = appId,
                Name = requestBody.Name
            };

            return HandleAsync(request);
        }

        [HttpPut("{appId:guid}/owner/{newOwnerId:guid}")]
        [Authorize]
        public Task<IResult> SetAppOwner(Guid appId, Guid newOwnerId)
        {
            var request = new SetAppOwnerRequest { AppId = appId, NewOwnerId = newOwnerId };

            return HandleAsync(request);
        }

        [HttpPut("{appId:guid}/token")]
        [Authorize]
        public Task<IResult<Guid>> UpdateAppApiToken(Guid appId)
        {
            var request = new UpdateAppApiTokenRequest { AppId = appId };

            return HandleAsync<UpdateAppApiTokenRequest, Guid>(request);
        }

        [HttpDelete("{appId:guid}")]
        [Authorize]
        public Task<IResult> DeleteApp(Guid appId)
        {
            var request = new DeleteAppRequest { AppId = appId };

            return HandleAsync(request);
        }
    }
}