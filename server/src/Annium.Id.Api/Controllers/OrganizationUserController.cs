using Annium.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("organizations/{organizationId:guid}/users/{userId:guid}")]
    public class OrganizationUserController : ServerController
    {
        public OrganizationUserController() { }

        [HttpPut]
        // TODO: Auth
        public IActionResult AddUserToOrganizationAsync()
        {
            return NoContent();
        }

        [HttpPut("roles/{roleId:guid}")]
        // TODO: Auth
        public IActionResult AddRoleToOrganizationUserAsync()
        {
            return NoContent();
        }

        [HttpDelete("roles/{roleId:guid}")]
        // TODO: Auth
        public IActionResult DeleteRoleFromOrganizationUserAsync()
        {
            return NoContent();
        }

        [HttpPut("claims/{claimId:guid}")]
        // TODO: Auth
        public IActionResult AddClaimToOrganizationUserAsync()
        {
            return NoContent();
        }

        [HttpDelete("claims/{claimId:guid}")]
        // TODO: Auth
        public IActionResult DeleteClaimFromOrganizationUserAsync()
        {
            return NoContent();
        }

        [HttpDelete]
        // TODO: Auth
        public IActionResult DeleteUserFromOrganizationAsync()
        {
            return NoContent();
        }
    }
}