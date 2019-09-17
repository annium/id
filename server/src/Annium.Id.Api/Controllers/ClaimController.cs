using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.AspNetCore;
using Annium.Id.ViewModels.Claims.Requests;
using Annium.Id.ViewModels.Claims.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("apps/{appId:guid}/claims")]
    public class ClaimController : ServerController
    {
        public ClaimController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost]
        [Authorize]
        public Task<IActionResult> CreateClaimAsync(Guid appId, [FromBody] CreateClaimRequest request)
        {
            request.AppId = appId;

            return HandleAsync<CreateClaimRequest, Guid>(request);
        }

        [HttpGet]
        [Authorize]
        public Task<IActionResult> ListClaimsAsync(Guid appId)
        {
            var request = new ListClaimsRequest { AppId = appId };

            return HandleAsync<ListClaimsRequest, IEnumerable<ClaimResponse>>(request);
        }

        [HttpPut("{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> UpdateClaimAsync(Guid appId, Guid claimId, [FromBody] UpdateClaimRequest request)
        {
            request.AppId = appId;
            request.ClaimId = claimId;

            return HandleAsync(request);
        }

        [HttpDelete("{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteClaimAsync(Guid appId, Guid claimId)
        {
            var request = new DeleteClaimRequest { AppId = appId, ClaimId = claimId };

            return HandleAsync(request);
        }
    }
}