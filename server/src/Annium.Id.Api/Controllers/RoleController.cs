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
        public Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            return HandleAsync<CreateRoleRequest, Guid>(request);
        }

        [HttpGet]
        [Authorize]
        public Task<IActionResult> ListRoles(Guid appId)
        {
            var request = new ListRolesRequest { AppId = appId };

            return HandleAsync<ListRolesRequest, IEnumerable<RoleResponse>>(request);
        }

        [HttpPut("{roleId:guid}")]
        [Authorize]
        public Task<IActionResult> UpdateRole(Guid roleId, [FromBody] UpdateRoleRequestBase requestBase)
        {
            var request = new UpdateRoleRequest
            {
                RoleId = roleId,
                Key = requestBase.Key,
                Name = requestBase.Name,
            };

            return HandleAsync(request);
        }

        [HttpPost("{roleId:guid}/claims/{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> AddClaimToRole(Guid roleId, Guid claimId, [FromBody] AddClaimToRoleRequestBase requestBase)
        {
            var request = new AddClaimToRoleRequest
            {
                RoleId = roleId,
                ClaimId = claimId,
                Value = requestBase.Value,
            };

            return HandleAsync(request);
        }

        [HttpDelete("{roleId:guid}/claims/{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteClaimFromRole(Guid roleId, Guid claimId)
        {
            var request = new DeleteClaimFromRoleRequest
            {
                RoleId = roleId,
                ClaimId = claimId
            };

            return HandleAsync(request);
        }

        [HttpDelete("{roleId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteRole(Guid roleId)
        {
            var request = new DeleteRoleRequest
            {
                RoleId = roleId
            };

            return HandleAsync(request);
        }
    }
}