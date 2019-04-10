using System;
using Annium.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("apps/{appId:guid}/claims")]
    public class ClaimController : ServerController
    {
        public ClaimController() { }

        [HttpPut]
        // TODO: Auth
        public IActionResult CreateClaimAsync(Guid appId)
        {
            return NoContent();
        }

        [HttpGet]
        // TODO: Auth
        public IActionResult ListClaimsAsync(Guid appId)
        {
            return NoContent();
        }

        [HttpPost("{claimId:guid}")]
        // TODO: Auth
        public IActionResult UpdateClaimAsync(Guid appId, Guid claimId)
        {
            return NoContent();
        }

        [HttpDelete("{claimId:guid}")]
        // TODO: Auth
        public IActionResult DeleteClaimAsync(Guid appId, Guid claimId)
        {
            return NoContent();
        }
    }
}