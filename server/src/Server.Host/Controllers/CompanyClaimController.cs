using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Server.ViewModels.Requests.CompanyClaims;
using Server.ViewModels.Responses.CompanyClaims;

namespace Server.Host.Controllers;

[Route("companies/claims")]
public class CompanyClaimController : ServerController
{
    public CompanyClaimController(IMediator mediator, IServiceProvider sp)
        : base(mediator, sp) { }

    [HttpPost]
    [Authorize]
    public Task<IResult<Guid>> CreateCompanyClaimAsync([FromBody] CreateCompanyClaimRequest request)
    {
        return HandleAsync<CreateCompanyClaimRequest, Guid>(request);
    }

    [HttpGet]
    [Authorize]
    public Task<IResult<IEnumerable<CompanyClaimResponse>>> ListCompanyClaimsAsync(Guid appId)
    {
        var request = new ListCompanyClaimsRequest { AppId = appId };

        return HandleAsync<ListCompanyClaimsRequest, IEnumerable<CompanyClaimResponse>>(request);
    }

    [HttpPut("{claimId:guid}")]
    [Authorize]
    public Task<IResult> UpdateCompanyClaimAsync(Guid claimId, [FromBody] UpdateCompanyClaimRequestBody requestBody)
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
    public Task<IResult> DeleteCompanyClaimAsync(Guid claimId)
    {
        var request = new DeleteCompanyClaimRequest { ClaimId = claimId };

        return HandleAsync(request);
    }
}
