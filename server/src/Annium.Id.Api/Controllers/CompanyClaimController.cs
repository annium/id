using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.AspNetCore;
using Annium.Id.ViewModels.CompanyClaims.Requests;
using Annium.Id.ViewModels.CompanyClaims.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    // TODO: change to apps/{appId:guid}/companies/claims
    [Route("apps/{appId:guid}/company-claims")]
    public class CompanyClaimController : ServerController
    {
        public CompanyClaimController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost]
        [Authorize]
        public Task<IActionResult> CreateCompanyClaimAsync(Guid appId, [FromBody] CreateCompanyClaimRequest request)
        {
            request.AppId = appId;

            return HandleAsync<CreateCompanyClaimRequest, Guid>(request);
        }

        [HttpGet]
        [Authorize]
        public Task<IActionResult> ListCompanyClaimsAsync(Guid appId)
        {
            var request = new ListCompanyClaimsRequest { AppId = appId };

            return HandleAsync<ListCompanyClaimsRequest, IEnumerable<CompanyClaimResponse>>(request);
        }

        [HttpPut("{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> UpdateCompanyClaimAsync(Guid appId, Guid claimId, [FromBody] UpdateCompanyClaimRequest request)
        {
            request.AppId = appId;
            request.ClaimId = claimId;

            return HandleAsync(request);
        }

        [HttpDelete("{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteCompanyClaimAsync(Guid appId, Guid claimId)
        {
            var request = new DeleteCompanyClaimRequest { AppId = appId, ClaimId = claimId };

            return HandleAsync(request);
        }
    }
}