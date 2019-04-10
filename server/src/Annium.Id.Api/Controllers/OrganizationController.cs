using Annium.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("organizations")]
    public class OrganizationControlller : ServerController
    {
        public OrganizationControlller() { }

        [HttpPut]
        public IActionResult RegisterOrganizationAsync()
        {
            return NoContent();
        }

        [HttpGet("{organizationId:guid}")]
        public IActionResult GetOrganizationInfoAsync()
        {
            return NoContent();
        }

        [HttpPost("{organizationId:guid}")]
        // TODO: Auth
        public IActionResult UpdateOrganizationAsync()
        {
            return NoContent();
        }

        [HttpPost("{organizationId:guid}/set-owner/{userId:guid}")]
        // TODO: Auth
        public IActionResult SetOrganizationOwnerAsync()
        {
            return NoContent();
        }

        [HttpDelete("{organizationId:guid}")]
        // TODO: Auth
        public IActionResult UnregisterOrganizationAsync()
        {
            return NoContent();
        }
    }
}