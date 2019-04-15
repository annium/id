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
    [Route("apps/{appId:guid}/company-claims")]
    public class CompanyClaimController : LocalizedServerController
    {
        private readonly IAppRepository appRepository;

        private readonly ICompanyClaimRepository claimRepository;

        public CompanyClaimController(
            IAppRepository appRepository,
            ICompanyClaimRepository claimRepository,
            IStringLocalizer<CompanyClaimController> localizer
        ) : base(localizer)
        {
            this.appRepository = appRepository;
            this.claimRepository = claimRepository;
        }

        [HttpPut]
        [AuthorizeId]
        public async Task<IActionResult> CreateCompanyClaimAsync(Guid appId, [FromBody] CompanyClaimPayload claimPayload)
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

            var claim = new CompanyClaim(
                app.Id,
                claimPayload.Key,
                claimPayload.Name
            );

            claim = await claimRepository.CreateAsync(claim);

            return Ok(new CompanyClaimView(claim));
        }

        [HttpGet]
        [AuthorizeId]
        public async Task<IActionResult> ListCompanyClaimsAsync(Guid appId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            var claims = await claimRepository.GetAllAsync(appId);

            return Ok(claims.Select(c => new CompanyClaimView(c)).ToArray());
        }

        [HttpPost("{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> UpdateCompanyClaimAsync(Guid appId, Guid claimId, [FromBody] CompanyClaimPayload claimPayload)
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

            return Ok(new CompanyClaimView(claim));
        }

        [HttpDelete("{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteCompanyClaimAsync(Guid appId, Guid claimId)
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