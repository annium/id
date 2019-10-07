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
    [Route("claims")]
    public class ClaimController : ServerController
    {
        public ClaimController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost]
        [Authorize]
        public Task<IActionResult> CreateClaim([FromBody] CreateClaimRequest request)
        {
            return HandleAsync<CreateClaimRequest, Guid>(request);
        }

        [HttpGet]
        [Authorize]
        public Task<IActionResult> ListClaims(Guid appId)
        {
            var request = new ListClaimsRequest { AppId = appId };

            return HandleAsync<ListClaimsRequest, IEnumerable<ClaimResponse>>(request);
        }

        [HttpPut("{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> UpdateClaim(Guid claimId, [FromBody] UpdateClaimRequestBase requestBase)
        {
            var request = new UpdateClaimRequest
            {
                ClaimId = claimId,
                Key = requestBase.Key,
                Name = requestBase.Name,
            };

            return HandleAsync(request);
        }

        [HttpDelete("{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteClaim(Guid claimId)
        {
            var request = new DeleteClaimRequest { ClaimId = claimId };

            return HandleAsync(request);
        }
    }
}