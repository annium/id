using System;
using Annium.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("apps/{appId:guid}/organization-roles")]
    public class OrganizationRoleController : ServerController
    {
        public OrganizationRoleController() { }

        [HttpPut]
        // TODO: Auth
        public IActionResult CreateOrganizationRoleAsync(Guid appId)
        {
            return NoContent();
        }

        [HttpGet]
        // TODO: Auth
        public IActionResult ListOrganizationRolesAsync(Guid appId)
        {
            // add info about role claims
            return NoContent();
        }

        [HttpPost("{roleId:guid}")]
        // TODO: Auth
        public IActionResult UpdateOrganizationRoleAsync(Guid appId, Guid roleId)
        {
            return NoContent();
        }

        [HttpPut("{roleId:guid}/claims/{claimId:guid}")]
        // TODO: Auth
        public IActionResult AddOrganizationRoleClaimAsync(Guid appId, Guid roleId, Guid claimId)
        {
            return NoContent();
        }

        [HttpDelete("{roleId:guid}/claims/{claimId:guid}")]
        // TODO: Auth
        public IActionResult DeleteOrganizationRoleClaimAsync(Guid appId, Guid roleId, Guid claimId)
        {
            return NoContent();
        }

        [HttpDelete("{roleId:guid}")]
        // TODO: Auth
        public IActionResult DeleteOrganizationRoleAsync(Guid appId, Guid roleId)
        {
            return NoContent();
        }
    }
}