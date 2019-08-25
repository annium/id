using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mapper;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Id.AspNetCore;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Annium.Id.Api.Controllers
{
    [Route("apps/{appId:guid}/claims")]
    public class ClaimController : LocalizedServerController
    {
        private readonly IAppRepository appRepository;
        private readonly IClaimRepository claimRepository;
        private readonly IMapper mapper;

        public ClaimController(
            IAppRepository appRepository,
            IClaimRepository claimRepository,
            IMapper mapper,
            IStringLocalizer<ClaimController> localizer
        ) : base(localizer)
        {
            this.appRepository = appRepository;
            this.claimRepository = claimRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateClaimAsync(Guid appId, [FromBody] ClaimPayload claimPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var(app, result) = await VerifyAppOwnerAsync(appId, "create claim");
            if (result != null)
                return result;

            if ((await claimRepository.FindByKeyAsync(app.Id, claimPayload.Key)) != null)
                return Conflict($"Claim key {claimPayload.Key} is already used");

            var claim = new Claim(
                app.Id,
                claimPayload.Key,
                claimPayload.Name
            );

            claim = await claimRepository.CreateAsync(claim);

            return Ok(mapper.Map<ClaimView>(claim));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListClaimsAsync(Guid appId)
        {
            var(app, result) = await VerifyAppAsync(appId);
            if (result != null)
                return result;

            var claims = await claimRepository.GetAllAsync(appId);

            return Ok(claims.Select(mapper.Map<ClaimView>).ToArray());
        }

        [HttpPut("{claimId:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateClaimAsync(Guid appId, Guid claimId, [FromBody] ClaimPayload claimPayload)
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

            return Ok(mapper.Map<ClaimView>(claim));
        }

        [HttpDelete("{claimId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteClaimAsync(Guid appId, Guid claimId)
        {
            var(app, claim, result) = await VerifyAppOwnerClaimAsync(appId, claimId, "delete claim");
            if (result != null)
                return result;

            await claimRepository.DeleteByIdAsync(claim.Id);

            return NoContent();
        }

        private async Task<ValueTuple<App, Claim, IActionResult>> VerifyAppOwnerClaimAsync(Guid appId, Guid claimId, string operation)
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