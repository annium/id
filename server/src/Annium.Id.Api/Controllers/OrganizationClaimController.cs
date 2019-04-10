using System;
using Annium.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("apps/{appId:guid}/organization-claims")]
    public class OrganizationClaimController : ServerController
    {
        public OrganizationClaimController() { }

        [HttpPut]
        // TODO: Auth
        public IActionResult CreateOrganizationClaimAsync(Guid appId)
        {
            return NoContent();
        }

        [HttpGet]
        // TODO: Auth
        public IActionResult ListOrganizationClaimsAsync(Guid appId)
        {
            return NoContent();
        }

        [HttpPost("{claimId:guid}")]
        // TODO: Auth
        public IActionResult UpdateOrganizationClaimAsync(Guid appId, Guid claimId)
        {
            return NoContent();
        }

        [HttpDelete("{claimId:guid}")]
        // TODO: Auth
        public IActionResult DeleteOrganizationClaimAsync(Guid appId, Guid claimId)
        {
            return NoContent();
        }
    }
}