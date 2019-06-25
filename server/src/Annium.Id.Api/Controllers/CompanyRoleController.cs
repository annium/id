using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Extensions.Mapper;
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
        private readonly IMapper mapper;

        public CompanyRoleController(
            IAppRepository appRepository,
            ICompanyRoleRepository roleRepository,
            ICompanyRoleClaimRepository roleClaimRepository,
            ICompanyClaimRepository claimRepository,
            IMapper mapper,
            IStringLocalizer<CompanyRoleController> localizer
        ) : base(localizer)
        {
            this.appRepository = appRepository;
            this.roleRepository = roleRepository;
            this.roleClaimRepository = roleClaimRepository;
            this.claimRepository = claimRepository;
            this.mapper = mapper;
        }

        [HttpPut]
        [AuthorizeId]
        public async Task<IActionResult> CreateRoleAsync(Guid appId, [FromBody] CompanyRolePayload rolePayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var(app, result) = await VerifyAppOwnerAsync(appId, "create role");
            if (result != null)
                return result;

            if ((await roleRepository.FindByKeyAsync(app.Id, rolePayload.Key)) != null)
                return Conflict($"Role key {rolePayload.Key} is already used");

            var role = new CompanyRole(
                app.Id,
                rolePayload.Key,
                rolePayload.Name,
                Array.Empty<ClaimValue>()
            );

            role = await roleRepository.CreateAsync(role);

            return Ok(mapper.Map<CompanyRoleView>(role));
        }

        [HttpGet]
        [AuthorizeId]
        public async Task<IActionResult> ListRolesAsync(Guid appId)
        {
            var(app, result) = await VerifyAppAsync(appId);
            if (result != null)
                return result;

            var roles = await roleRepository.GetAllAsync(appId);

            return Ok(roles);
        }

        [HttpPost("{roleId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> UpdateRoleAsync(Guid appId, Guid roleId, [FromBody] CompanyRolePayload rolePayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var(app, role, result) = await VerifyAppOwnerRoleAsync(appId, roleId, "create role");
            if (result != null)
                return result;

            if (rolePayload.Key != role.Key && (await roleRepository.FindByKeyAsync(app.Id, rolePayload.Key)) != null)
                return Conflict($"Role key {rolePayload.Key} is already used");

            role.Key = rolePayload.Key;
            role.Name = rolePayload.Name;

            role = await roleRepository.UpdateAsync(role);

            return Ok(mapper.Map<CompanyRoleView>(role));
        }

        [HttpPost("{roleId:guid}/claims/{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> AddClaimToRoleAsync(Guid appId, Guid roleId, Guid claimId, [FromBody] ClaimValuePayload valuePayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var(app, role, claim, result) = await VerifyAppOwnerRoleClaimAsync(appId, roleId, claimId, "add claim to role");
            if (result != null)
                return result;

            var roleClaim = new CompanyRoleClaim(role.Id, claim.Id, valuePayload.Value);

            roleClaim = await roleClaimRepository.SaveAsync(roleClaim);

            return Ok(new ClaimValue(claim.Id, claim.Key, claim.Name, roleClaim.Value));
        }

        [HttpDelete("{roleId:guid}/claims/{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteClaimFromRoleAsync(Guid appId, Guid roleId, Guid claimId)
        {
            var(app, role, claim, result) = await VerifyAppOwnerRoleClaimAsync(appId, roleId, claimId, "delete claim from role");
            if (result != null)
                return result;

            await roleClaimRepository.DeleteByIdAsync(role.Id, claim.Id);

            return NoContent();
        }

        [HttpDelete("{roleId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteRoleAsync(Guid appId, Guid roleId)
        {
            var(app, role, result) = await VerifyAppOwnerRoleAsync(appId, roleId, "delete role");
            if (result != null)
                return result;

            await roleRepository.DeleteByIdAsync(role.Id);

            return NoContent();
        }

        private async Task<ValueTuple<App, CompanyRole, CompanyClaim, IActionResult>> VerifyAppOwnerRoleClaimAsync(Guid appId, Guid roleId, Guid claimId, string operation)
        {
            var(app, role, result) = await VerifyAppOwnerRoleAsync(appId, roleId, operation);
            if (result != null)
                return (null, null, null, result);

            var claim = await claimRepository.GetByIdAsync(claimId);
            if (claim == null)
                return (null, null, null, NotFound("Claim not found"));

            if (claim.AppId != app.Id)
                return (null, null, null, Forbidden("Claim belongs to another application"));

            return (app, role, claim, null);
        }

        private async Task<ValueTuple<App, CompanyRole, IActionResult>> VerifyAppOwnerRoleAsync(Guid appId, Guid roleId, string operation)
        {
            var(app, result) = await VerifyAppOwnerAsync(appId, operation);
            if (result != null)
                return (null, null, result);

            var role = await roleRepository.GetByIdAsync(roleId);
            if (role == null)
                return (null, null, NotFound("Role not found"));

            if (role.AppId != app.Id)
                return (null, null, Forbidden("Role belongs to another application"));

            return (app, role, null);
        }

        private async Task<ValueTuple<App, IActionResult>> VerifyAppOwnerAsync(Guid appId, string operation)
        {
            var(app, result) = await VerifyAppAsync(appId);
            if (result != null)
                return (null, result);

            if (this.GetId().UserId != app.OwnerId)
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