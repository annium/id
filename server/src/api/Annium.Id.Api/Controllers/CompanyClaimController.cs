using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.AspNetCore;
using Annium.Id.Api.ViewModels.CompanyClaims.Requests;
using Annium.Id.Api.ViewModels.CompanyClaims.Responses;
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
        public Task<IResult<Guid>> CreateCompanyClaim([FromBody] CreateCompanyClaimRequest request)
        {
            return HandleAsync<CreateCompanyClaimRequest, Guid>(request);
        }

        [HttpGet]
        [Authorize]
        public Task<IResult<IEnumerable<CompanyClaimResponse>>> ListCompanyClaims(Guid appId)
        {
            var request = new ListCompanyClaimsRequest { AppId = appId };

            return HandleAsync<ListCompanyClaimsRequest, IEnumerable<CompanyClaimResponse>>(request);
        }

        [HttpPut("{claimId:guid}")]
        [Authorize]
        public Task<IResult> UpdateCompanyClaim(Guid claimId, [FromBody] UpdateCompanyClaimRequestBody requestBody)
        {
            var request = new UpdateCompanyClaimRequest
            {
                ClaimId = claimId,
                Key = requestBody.Key,
                Name = requestBody.Name,
            };

            return HandleAsync(request);
        }

        [HttpDelete("{claimId:guid}")]
        [Authorize]
        public Task<IResult> DeleteCompanyClaim(Guid claimId)
        {
            var request = new DeleteCompanyClaimRequest { ClaimId = claimId };

            return HandleAsync(request);
        }
    }
}