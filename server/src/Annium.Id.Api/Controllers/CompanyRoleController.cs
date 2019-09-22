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
    [Route("companies/roles")]
    public class CompanyRoleController : ServerController
    {
        public CompanyRoleController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost]
        [Authorize]
        public Task<IActionResult> CreateRoleAsync([FromBody] CreateCompanyRoleRequest request)
        {
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
        public Task<IActionResult> UpdateRoleAsync(Guid roleId, [FromBody] UpdateCompanyRoleRequest request)
        {
            request.RoleId = roleId;

            return HandleAsync(request);
        }

        [HttpPost("{roleId:guid}/claims/{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> AddClaimToRoleAsync(Guid roleId, Guid claimId, [FromBody] AddCompanyClaimToCompanyRoleRequest request)
        {
            request.RoleId = roleId;
            request.ClaimId = claimId;

            return HandleAsync(request);
        }

        [HttpDelete("{roleId:guid}/claims/{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteClaimFromRoleAsync(Guid roleId, Guid claimId)
        {
            var request = new DeleteCompanyClaimFromCompanyRoleRequest { RoleId = roleId, ClaimId = claimId };

            return HandleAsync(request);
        }

        [HttpDelete("{roleId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteRoleAsync(Guid roleId)
        {
            var request = new DeleteCompanyRoleRequest { RoleId = roleId };

            return HandleAsync(request);
        }
    }
}