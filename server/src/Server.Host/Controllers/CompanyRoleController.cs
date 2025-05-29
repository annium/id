using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Server.ViewModels.Requests.CompanyRoles;
using Server.ViewModels.Responses.CompanyRoles;

namespace Server.Host.Controllers;

[Route("companies/roles")]
public class CompanyRoleController : ServerController
{
    public CompanyRoleController(IMediator mediator, IServiceProvider sp)
        : base(mediator, sp) { }

    [HttpPost]
    [Authorize]
    public Task<IResult<Guid>> CreateRoleAsync([FromBody] CreateCompanyRoleRequest request)
    {
        return HandleAsync<CreateCompanyRoleRequest, Guid>(request);
    }

    [HttpGet]
    [Authorize]
    public Task<IResult<IEnumerable<CompanyRoleResponse>>> ListRolesAsync(Guid appId)
    {
        var request = new ListCompanyRolesRequest { AppId = appId };

        return HandleAsync<ListCompanyRolesRequest, IEnumerable<CompanyRoleResponse>>(request);
    }

    [HttpPut("{roleId:guid}")]
    [Authorize]
    public Task<IResult> UpdateRoleAsync(Guid roleId, [FromBody] UpdateCompanyRoleRequestBody requestBody)
    {
        var request = new UpdateCompanyRoleRequest
        {
            RoleId = roleId,
            Key = requestBody.Key,
            Name = requestBody.Name,
        };

        return HandleAsync(request);
    }

    [HttpPost("{roleId:guid}/claims/{claimId:guid}")]
    [Authorize]
    public Task<IResult> AddClaimToRoleAsync(
        Guid roleId,
        Guid claimId,
        [FromBody] AddCompanyClaimToCompanyRoleRequestBody requestBody
    )
    {
        var request = new AddCompanyClaimToCompanyRoleRequest
        {
            RoleId = roleId,
            ClaimId = claimId,
            Value = requestBody.Value,
        };

        return HandleAsync(request);
    }

    [HttpDelete("{roleId:guid}/claims/{claimId:guid}")]
    [Authorize]
    public Task<IResult> DeleteClaimFromRoleAsync(Guid roleId, Guid claimId)
    {
        var request = new DeleteCompanyClaimFromCompanyRoleRequest { RoleId = roleId, ClaimId = claimId };

        return HandleAsync(request);
    }

    [HttpDelete("{roleId:guid}")]
    [Authorize]
    public Task<IResult> DeleteRoleAsync(Guid roleId)
    {
        var request = new DeleteCompanyRoleRequest { RoleId = roleId };

        return HandleAsync(request);
    }
}
