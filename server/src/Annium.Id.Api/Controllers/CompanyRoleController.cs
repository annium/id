using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.AspNetCore;
using Annium.Id.ViewModels.CompanyRoles.Requests;
using Annium.Id.ViewModels.CompanyRoles.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    // TODO: change to apps/{appId:guid}/companies/roles
    [Route("apps/{appId:guid}/company-roles")]
    public class CompanyRoleController : ServerController
    {
        public CompanyRoleController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost]
        [Authorize]
        public Task<IActionResult> CreateRoleAsync(Guid appId, [FromBody] CreateCompanyRoleRequest request)
        {
            request.AppId = appId;

            return HandleAsync<CreateCompanyRoleRequest, Guid>(request);
        }

        [HttpGet]
        [Authorize]
        public Task<IActionResult> ListRolesAsync(Guid appId)
        {
            var request = new ListCompanyRolesRequest { AppId = appId };

            return HandleAsync<ListCompanyRolesRequest, IEnumerable<CompanyRoleResponse>>(request);
        }

        [HttpPut("{roleId:guid}")]
        [Authorize]
        public Task<IActionResult> UpdateRoleAsync(Guid appId, Guid roleId, [FromBody] UpdateCompanyRoleRequest request)
        {
            request.AppId = appId;
            request.RoleId = roleId;

            return HandleAsync(request);
        }

        [HttpPost("{roleId:guid}/claims/{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> AddClaimToRoleAsync(Guid appId, Guid roleId, Guid claimId, [FromBody] AddCompanyClaimToCompanyRoleRequest request)
        {
            request.AppId = appId;
            request.RoleId = roleId;
            request.ClaimId = claimId;

            return HandleAsync(request);
        }

        [HttpDelete("{roleId:guid}/claims/{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteClaimFromRoleAsync(Guid appId, Guid roleId, Guid claimId)
        {
            var request = new DeleteCompanyClaimFromCompanyRoleRequest();
            request.AppId = appId;
            request.RoleId = roleId;
            request.ClaimId = claimId;

            return HandleAsync(request);
        }

        [HttpDelete("{roleId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteRoleAsync(Guid appId, Guid roleId)
        {
            var request = new DeleteCompanyRoleRequest();
            request.AppId = appId;
            request.RoleId = roleId;

            return HandleAsync(request);
        }
    }
}