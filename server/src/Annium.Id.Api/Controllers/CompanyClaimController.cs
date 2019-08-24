using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mapper;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Id.AspNetCore;
using Annium.Id.Db;
using Annium.Id.Db.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Annium.Id.Api.Controllers
{
    // TODO: change to apps/{appId:guid}/companies/claims
    [Route("apps/{appId:guid}/company-claims")]
    public class CompanyClaimController : LocalizedServerController
    {
        private readonly IAppRepository appRepository;
        private readonly ICompanyClaimRepository claimRepository;
        private readonly IMapper mapper;

        public CompanyClaimController(
            IAppRepository appRepository,
            ICompanyClaimRepository claimRepository,
            IMapper mapper,
            IStringLocalizer<CompanyClaimController> localizer
        ) : base(localizer)
        {
            this.appRepository = appRepository;
            this.claimRepository = claimRepository;
            this.mapper = mapper;
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> CreateCompanyClaimAsync(Guid appId, [FromBody] CompanyClaimPayload claimPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var(app, result) = await VerifyAppOwnerAsync(appId, "create claim");
            if (result != null)
                return result;

            if ((await claimRepository.FindByKeyAsync(app.Id, claimPayload.Key)) != null)
                return Conflict($"Claim key {claimPayload.Key} is already used");

            var claim = new CompanyClaim(app.Id, claimPayload.Key, claimPayload.Name);
            claim = await claimRepository.CreateAsync(claim);

            return Ok(mapper.Map<CompanyClaimView>(claim));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListCompanyClaimsAsync(Guid appId)
        {
            var(app, result) = await VerifyAppAsync(appId);
            if (result != null)
                return result;

            var claims = await claimRepository.GetAllAsync(appId);

            return Ok(claims.Select(mapper.Map<CompanyClaimView>).ToArray());
        }

        [HttpPost("{claimId:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateCompanyClaimAsync(Guid appId, Guid claimId, [FromBody] CompanyClaimPayload claimPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var(app, claim, result) = await VerifyAppOwnerClaimAsync(appId, claimId, "update claim");
            if (result != null)
                return result;

            if (claimPayload.Key != claim.Key && (await claimRepository.FindByKeyAsync(app.Id, claimPayload.Key)) != null)
                return Conflict($"Claim key {claimPayload.Key} is already used");

            claim.Key = claimPayload.Key;
            claim.Name = claimPayload.Name;

            claim = await claimRepository.UpdateAsync(claim);

            return Ok(mapper.Map<CompanyClaimView>(claim));
        }

        [HttpDelete("{claimId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteCompanyClaimAsync(Guid appId, Guid claimId)
        {
            var(app, claim, result) = await VerifyAppOwnerClaimAsync(appId, claimId, "delete claim");
            if (result != null)
                return result;

            await claimRepository.DeleteByIdAsync(claim.Id);

            return NoContent();
        }

        private async Task<ValueTuple<App, CompanyClaim, IActionResult>> VerifyAppOwnerClaimAsync(Guid appId, Guid claimId, string operation)
        {
            var(app, result) = await VerifyAppOwnerAsync(appId, operation);
            if (result != null)
                return (null, null, result);

            var claim = await claimRepository.GetByIdAsync(claimId);
            if (claim == null)
                return (null, null, NotFound("Claim not found"));

            if (claim.AppId != app.Id)
                return (null, null, Forbidden("Claim belongs to another application"));

            return (app, claim, null);
        }

        private async Task<ValueTuple<App, IActionResult>> VerifyAppOwnerAsync(Guid appId, string operation)
        {
            var(app, result) = await VerifyAppAsync(appId);
            if (result != null)
                return (null, result);

            if (this.GetBaseId().UserId != app.OwnerId)
                return (null, Forbidden($"Need to be application owner to {operation}"));

            return (app, null);
        }

        private async Task<ValueTuple<App, IActionResult>> VerifyAppAsync(Guid appId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return (null, NotFound("Application not found"));

            return (app, null);
        }
    }
}