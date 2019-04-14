using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Id.AspNetCore;
using Annium.Id.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Annium.Id.Api.Controllers
{
    [Route("apps/{appId:guid}/claims")]
    public class ClaimController : LocalizedServerController
    {
        private readonly IAppRepository appRepository;

        private readonly IClaimRepository claimRepository;

        public ClaimController(
            IAppRepository appRepository,
            IClaimRepository claimRepository,
            IStringLocalizer<ClaimController> localizer
        ) : base(localizer)
        {
            this.appRepository = appRepository;
            this.claimRepository = claimRepository;
        }

        [HttpPut]
        [AuthorizeId]
        public async Task<IActionResult> CreateClaimAsync(Guid appId, [FromBody] ClaimPayload claimPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be application owner to create claim");

            if ((await claimRepository.FindByKeyAsync(app.Id, claimPayload.Key)) != null)
                return Conflict($"Claim key {claimPayload.Key} is already used");

            var claim = new Claim(
                app.Id,
                claimPayload.Key,
                claimPayload.Name
            );

            claim = await claimRepository.CreateAsync(claim);

            return Ok(new ClaimView(claim));
        }

        [HttpGet]
        [AuthorizeId]
        public async Task<IActionResult> ListClaimsAsync(Guid appId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            var claims = await claimRepository.GetAllAsync(appId);

            return Ok(claims.Select(c => new ClaimView(c)).ToArray());
        }

        [HttpPost("{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> UpdateClaimAsync(Guid appId, Guid claimId, [FromBody] ClaimPayload claimPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be application owner to update claim");

            var claim = await claimRepository.GetByIdAsync(claimId);
            if (claim == null)
                return NotFound();

            if (claim.AppId != app.Id)
                return Forbidden("Claim belongs to another application");

            if (claimPayload.Key != claim.Key && (await claimRepository.FindByKeyAsync(app.Id, claimPayload.Key)) != null)
                return Conflict($"Claim key {claimPayload.Key} is already used");

            claim.Key = claimPayload.Key;
            claim.Name = claimPayload.Name;

            claim = await claimRepository.UpdateAsync(claim);

            return Ok(new ClaimView(claim));
        }

        [HttpDelete("{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteClaimAsync(Guid appId, Guid claimId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be application owner to delete claim");

            var claim = await claimRepository.GetByIdAsync(claimId);
            if (claim == null)
                return NotFound();

            if (claim.AppId != app.Id)
                return Forbidden("Claim belongs to another application");

            await claimRepository.DeleteByIdAsync(claim.Id);

            return NoContent();
        }
    }
}