using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Claims;
using Annium.Id.Api.ViewModels.Responses.Claims;
using Annium.Id.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("claims")]
    public class ClaimController : ServerController
    {
        public ClaimController(
            IMediator mediator,
            IServiceProvider sp
        ) : base(mediator, sp)
        {
        }

        [HttpPost]
        [Authorize]
        public Task<IResult<Guid>> CreateClaim([FromBody] CreateClaimRequest request)
        {
            return HandleAsync<CreateClaimRequest, Guid>(request);
        }

        [HttpGet]
        [Authorize]
        public Task<IResult<IEnumerable<ClaimResponse>>> ListClaims(Guid appId)
        {
            var request = new ListClaimsRequest { AppId = appId };

            return HandleAsync<ListClaimsRequest, IEnumerable<ClaimResponse>>(request);
        }

        [HttpPut("{claimId:guid}")]
        [Authorize]
        public Task<IResult> UpdateClaim(Guid claimId, [FromBody] UpdateClaimRequestBody requestBody)
        {
            var request = new UpdateClaimRequest
            {
                ClaimId = claimId,
                Key = requestBody.Key,
                Name = requestBody.Name
            };

            return HandleAsync(request);
        }

        [HttpDelete("{claimId:guid}")]
        [Authorize]
        public Task<IResult> DeleteClaim(Guid claimId)
        {
            var request = new DeleteClaimRequest { ClaimId = claimId };

            return HandleAsync(request);
        }
    }
}