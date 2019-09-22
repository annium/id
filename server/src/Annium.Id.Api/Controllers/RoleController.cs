using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.AspNetCore;
using Annium.Id.ViewModels.Roles.Requests;
using Annium.Id.ViewModels.Roles.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("roles")]
    public class RoleController : ServerController
    {
        public RoleController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost]
        [Authorize]
        public Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleRequest request)
        {
            return HandleAsync<CreateRoleRequest, Guid>(request);
        }

        [HttpGet]
        [Authorize]
        public Task<IActionResult> ListRolesAsync(Guid appId)
        {
            var request = new ListRolesRequest { AppId = appId };

            return HandleAsync<ListRolesRequest, IEnumerable<RoleResponse>>(request);
        }

        [HttpPut("{roleId:guid}")]
        [Authorize]
        public Task<IActionResult> UpdateRoleAsync(Guid roleId, [FromBody] UpdateRoleRequest request)
        {
            request.RoleId = roleId;

            return HandleAsync(request);
        }

        [HttpPost("{roleId:guid}/claims/{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> AddClaimToRoleAsync(Guid roleId, Guid claimId, [FromBody] AddClaimToRoleRequest request)
        {
            request.RoleId = roleId;
            request.ClaimId = claimId;

            return HandleAsync(request);
        }

        [HttpDelete("{roleId:guid}/claims/{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteClaimFromRoleAsync(Guid roleId, Guid claimId)
        {
            var request = new DeleteClaimFromRoleRequest();
            request.RoleId = roleId;
            request.ClaimId = claimId;

            return HandleAsync(request);
        }

        [HttpDelete("{roleId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteRoleAsync(Guid roleId)
        {
            var request = new DeleteRoleRequest();
            request.RoleId = roleId;

            return HandleAsync(request);
        }
    }
}