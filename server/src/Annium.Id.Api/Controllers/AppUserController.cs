using Annium.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("apps/{appId:guid}/users/{userId:guid}")]
    public class AppUserController : ServerController
    {
        public AppUserController() { }

        [HttpPut("roles/{roleId:guid}")]
        // TODO: Auth
        public IActionResult AddRoleToUserRAsync()
        {
            return NoContent();
        }

        [HttpDelete("roles/{roleId:guid}")]
        // TODO: Auth
        public IActionResult DeleteRoleFromUserAsync()
        {
            return NoContent();
        }

        [HttpPut("claims/{claimId:guid}")]
        // TODO: Auth
        public IActionResult AddClaimToUserRAsync()
        {
            return NoContent();
        }

        [HttpDelete("claims/{claimId:guid}")]
        // TODO: Auth
        public IActionResult DeleteClaimFromUserAsync()
        {
            return NoContent();
        }
    }
}