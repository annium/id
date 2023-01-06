using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.CompanyUsers;
using Annium.Id.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers;

[Route("companies/{companyId:guid}/users/{userId:guid}")]
public class CompanyUserController : ServerController
{
    public CompanyUserController(
        IMediator mediator,
        IServiceProvider sp
    ) : base(mediator, sp)
    {
    }

    [HttpPost]
    [Authorize]
    public Task<IResult> AddUserToCompany(Guid companyId, Guid userId)
    {
        var request = new AddUserToCompanyRequest { CompanyId = companyId, UserId = userId };

        return HandleAsync(request);
    }

    [HttpPost("roles/{roleId:guid}")]
    [Authorize]
    public Task<IResult> AddCompanyRoleToCompanyUser(Guid companyId, Guid userId, Guid roleId)
    {
        var request = new AddCompanyRoleToCompanyUserRequest { CompanyId = companyId, UserId = userId, RoleId = roleId };

        return HandleAsync(request);
    }

    [HttpDelete("roles/{roleId:guid}")]
    [Authorize]
    public Task<IResult> DeleteCompanyRoleFromCompanyUser(Guid companyId, Guid userId, Guid roleId)
    {
        var request = new DeleteCompanyRoleFromCompanyUserRequest { CompanyId = companyId, UserId = userId, RoleId = roleId };

        return HandleAsync(request);
    }

    [HttpPost("claims/{claimId:guid}")]
    [Authorize]
    public Task<IResult> AddCompanyClaimToCompanyUser(Guid companyId, Guid userId, Guid claimId,
        [FromBody] AddCompanyClaimToCompanyUserRequestBody requestBody)
    {
        var request = new AddCompanyClaimToCompanyUserRequest
        {
            CompanyId = companyId,
            UserId = userId,
            ClaimId = claimId,
            Value = requestBody.Value
        };

        return HandleAsync(request);
    }

    [HttpDelete("claims/{claimId:guid}")]
    [Authorize]
    public Task<IResult> DeleteCompanyClaimFromCompanyUser(Guid companyId, Guid userId, Guid claimId)
    {
        var request = new DeleteCompanyClaimFromCompanyUserRequest { CompanyId = companyId, UserId = userId, ClaimId = claimId };

        return HandleAsync(request);
    }

    [HttpDelete]
    [Authorize]
    public Task<IResult> DeleteUserFromCompany(Guid companyId, Guid userId)
    {
        var request = new DeleteUserFromCompanyRequest { CompanyId = companyId, UserId = userId };

        return HandleAsync(request);
    }
}