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
    [Route("companies/claims")]
    public class CompanyClaimController : ServerController
    {
        public CompanyClaimController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost]
        [Authorize]
        public Task<IActionResult> CreateCompanyClaimAsync([FromBody] CreateCompanyClaimRequest request)
        {
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
        public Task<IActionResult> UpdateCompanyClaimAsync(Guid claimId, [FromBody] UpdateCompanyClaimRequestBase requestBase)
        {
            var request = new UpdateCompanyClaimRequest
            {
                ClaimId = claimId,
                Key = requestBase.Key,
                Name = requestBase.Name,
            };

            return HandleAsync(request);
        }

        [HttpDelete("{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteCompanyClaimAsync(Guid claimId)
        {
            var request = new DeleteCompanyClaimRequest { ClaimId = claimId };

            return HandleAsync(request);
        }
    }
}