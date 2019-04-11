using System;
using Annium.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("apps/{appId:guid}/roles")]
    public class RoleController : ServerController
    {
        public RoleController() { }

        [HttpPut]
        // TODO: Auth
        public IActionResult CreateRoleAsync(Guid appId)
        {
            return NoContent();
        }

        [HttpGet]
        // TODO: Auth
        public IActionResult ListRolesAsync(Guid appId)
        {
            // add info about role claims
            return NoContent();
        }

        [HttpPost("{roleId:guid}")]
        // TODO: Auth
        public IActionResult UpdateRoleAsync(Guid appId, Guid roleId)
        {
            return NoContent();
        }

        [HttpPut("{roleId:guid}/claims/{claimId:guid}")]
        // TODO: Auth
        public IActionResult AddRoleClaimAsync(Guid appId, Guid roleId, Guid claimId)
        {
            return NoContent();
        }

        [HttpDelete("{roleId:guid}/claims/{claimId:guid}")]
        // TODO: Auth
        public IActionResult DeleteRoleClaimAsync(Guid appId, Guid roleId, Guid claimId)
        {
            return NoContent();
        }

        [HttpDelete("{roleId:guid}")]
        // TODO: Auth
        public IActionResult DeleteRoleAsync(Guid appId, Guid roleId)
        {
            return NoContent();
        }
    }
}