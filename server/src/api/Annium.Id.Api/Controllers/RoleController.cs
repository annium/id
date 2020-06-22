using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.AspNetCore;
using Annium.Id.Api.ViewModels.Roles.Requests;
using Annium.Id.Api.ViewModels.Roles.Responses;
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
        public Task<IResult<Guid>> CreateRole([FromBody] CreateRoleRequest request)
        {
            return HandleAsync<CreateRoleRequest, Guid>(request);
        }

        [HttpGet]
        [Authorize]
        public Task<IResult<IEnumerable<RoleResponse>>> ListRoles(Guid appId)
        {
            var request = new ListRolesRequest { AppId = appId };

            return HandleAsync<ListRolesRequest, IEnumerable<RoleResponse>>(request);
        }

        [HttpPut("{roleId:guid}")]
        [Authorize]
        public Task<IResult> UpdateRole(Guid roleId, [FromBody] UpdateRoleRequestBody RequestBody)
        {
            var request = new UpdateRoleRequest
            {
                RoleId = roleId,
                Key = RequestBody.Key,
                Name = RequestBody.Name,
            };

            return HandleAsync(request);
        }

        [HttpPost("{roleId:guid}/claims/{claimId:guid}")]
        [Authorize]
        public Task<IResult> AddClaimToRole(Guid roleId, Guid claimId, [FromBody] AddClaimToRoleRequestBody RequestBody)
        {
            var request = new AddClaimToRoleRequest
            {
                RoleId = roleId,
                ClaimId = claimId,
                Value = RequestBody.Value,
            };

            return HandleAsync(request);
        }

        [HttpDelete("{roleId:guid}/claims/{claimId:guid}")]
        [Authorize]
        public Task<IResult> DeleteClaimFromRole(Guid roleId, Guid claimId)
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
        public Task<IResult> DeleteRole(Guid roleId)
        {
            var request = new DeleteRoleRequest
            {
                RoleId = roleId
            };

            return HandleAsync(request);
        }
    }
}