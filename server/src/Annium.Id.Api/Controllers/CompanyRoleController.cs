using System;
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
    [Route("apps/{appId:guid}/company-roles")]
    public class CompanyRoleController : LocalizedServerController
    {
        private readonly IAppRepository appRepository;

        private readonly ICompanyRoleRepository roleRepository;

        private readonly ICompanyRoleClaimRepository roleClaimRepository;

        private readonly ICompanyClaimRepository claimRepository;

        public CompanyRoleController(
            IAppRepository appRepository,
            ICompanyRoleRepository roleRepository,
            ICompanyRoleClaimRepository roleClaimRepository,
            ICompanyClaimRepository claimRepository,
            IStringLocalizer<CompanyRoleController> localizer
        ) : base(localizer)
        {
            this.appRepository = appRepository;
            this.roleRepository = roleRepository;
            this.roleClaimRepository = roleClaimRepository;
            this.claimRepository = claimRepository;
        }

        [HttpPut]
        [AuthorizeId]
        public async Task<IActionResult> CreateRoleAsync(Guid appId, [FromBody] CompanyRolePayload rolePayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be application owner to create role");

            if ((await roleRepository.FindByKeyAsync(app.Id, rolePayload.Key)) != null)
                return Conflict($"Role key {rolePayload.Key} is already used");

            var role = new CompanyRole(
                app.Id,
                rolePayload.Key,
                rolePayload.Name,
                Array.Empty<ClaimValue>()
            );

            role = await roleRepository.CreateAsync(role);

            return Ok(new CompanyRoleView(role));
        }

        [HttpGet]
        [AuthorizeId]
        public async Task<IActionResult> ListRolesAsync(Guid appId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            var roles = await roleRepository.GetAllAsync(appId);

            return Ok(roles);
        }

        [HttpPost("{roleId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> UpdateRoleAsync(Guid appId, Guid roleId, [FromBody] CompanyRolePayload rolePayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be application owner to update role");

            var role = await roleRepository.GetByIdAsync(roleId);
            if (role == null)
                return NotFound();

            if (role.AppId != app.Id)
                return Forbidden("Role belongs to another application");

            if (rolePayload.Key != role.Key && (await roleRepository.FindByKeyAsync(app.Id, rolePayload.Key)) != null)
                return Conflict($"Role key {rolePayload.Key} is already used");

            role.Key = rolePayload.Key;
            role.Name = rolePayload.Name;

            role = await roleRepository.UpdateAsync(role);

            return Ok(new CompanyRoleView(role));
        }

        [HttpPost("{roleId:guid}/claims/{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> AddClaimToRoleAsync(Guid appId, Guid roleId, Guid claimId, [FromBody] ClaimValuePayload valuePayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be application owner to add claim to role");

            var role = await roleRepository.GetByIdAsync(roleId);
            if (role == null)
                return NotFound();

            if (role.AppId != app.Id)
                return Forbidden("Role belongs to another application");

            var claim = await claimRepository.GetByIdAsync(claimId);
            if (claim == null)
                return NotFound();

            if (claim.AppId != app.Id)
                return Forbidden("Claim belongs to another application");

            var roleClaim = new CompanyRoleClaim(role.Id, claim.Id, valuePayload.Value);

            roleClaim = await roleClaimRepository.SaveAsync(roleClaim);

            return Ok(new ClaimValue(claim.Id, claim.Key, claim.Name, roleClaim.Value));
        }

        [HttpDelete("{roleId:guid}/claims/{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteClaimFromRoleAsync(Guid appId, Guid roleId, Guid claimId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be application owner to delete claim from role");

            var role = await roleRepository.GetByIdAsync(roleId);
            if (role == null)
                return NotFound();

            if (role.AppId != app.Id)
                return Forbidden("Role belongs to another application");

            var claim = await claimRepository.GetByIdAsync(claimId);
            if (claim == null)
                return NotFound();

            if (claim.AppId != app.Id)
                return Forbidden("Claim belongs to another application");

            await roleClaimRepository.DeleteByIdAsync(role.Id, claim.Id);

            return NoContent();
        }

        [HttpDelete("{roleId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteRoleAsync(Guid appId, Guid roleId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be application owner to delete role");

            var role = await roleRepository.GetByIdAsync(roleId);
            if (role == null)
                return NotFound();

            if (role.AppId != app.Id)
                return Forbidden("Role belongs to another application");

            await roleRepository.DeleteByIdAsync(role.Id);

            return NoContent();
        }
    }
}