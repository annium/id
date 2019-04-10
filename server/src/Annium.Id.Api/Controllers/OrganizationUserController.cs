using Annium.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("organizations/{organizationId:guid}")]
    public class OrganizationUserController : ServerController
    {
        public OrganizationUserController() { }

        [HttpPut("users/{userId:guid}")]
        // TODO: Auth
        public IActionResult AddUserToOrganizationAsync()
        {
            return NoContent();
        }

        [HttpPut("users/{userId:guid}/roles/{roleId:guid}")]
        // TODO: Auth
        public IActionResult AddRoleToOrganizationUserAsync()
        {
            return NoContent();
        }

        [HttpDelete("users/{userId:guid}/roles/{roleId:guid}")]
        // TODO: Auth
        public IActionResult DeleteRoleFromOrganizationUserAsync()
        {
            return NoContent();
        }

        [HttpPut("users/{userId:guid}/claims/{claimId:guid}")]
        // TODO: Auth
        public IActionResult AddClaimToOrganizationUserAsync()
        {
            return NoContent();
        }

        [HttpDelete("users/{userId:guid}/claims/{claimId:guid}")]
        // TODO: Auth
        public IActionResult DeleteClaimFromOrganizationUserAsync()
        {
            return NoContent();
        }

        [HttpDelete("users/{userId:guid}")]
        // TODO: Auth
        public IActionResult DeleteUserFromOrganizationAsync()
        {
            return NoContent();
        }
    }
}