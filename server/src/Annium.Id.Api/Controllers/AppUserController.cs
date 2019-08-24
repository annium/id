using System;
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
    [Route("apps/{appId:guid}/users/{userId:guid}")]
    public class AppUserController : LocalizedServerController
    {
        private readonly IAppRepository appRepository;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IClaimRepository claimRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IUserClaimRepository userClaimRepository;
        private readonly IMapper mapper;

        public AppUserController(
            IAppRepository appRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IClaimRepository claimRepository,
            IUserRoleRepository userRoleRepository,
            IUserClaimRepository userClaimRepository,
            IMapper mapper,
            IStringLocalizer<AppUserController> localizer
        ) : base(localizer)
        {
            this.appRepository = appRepository;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.claimRepository = claimRepository;
            this.userRoleRepository = userRoleRepository;
            this.userClaimRepository = userClaimRepository;
            this.mapper = mapper;
        }

        [HttpPut("roles/{roleId:guid}")]
        [Authorize]
        public async Task<IActionResult> AddRoleToUserAsync(Guid appId, Guid userId, Guid roleId)
        {
            var(app, user, role, result) = await VerifyAppOwnerUserRoleAsync(appId, userId, roleId, "add role to user");
            if (result != null)
                return result;

            var userRole = new UserRole(user.Id, role.Id);

            userRole = await userRoleRepository.SaveAsync(userRole);

            return NoContent();
        }

        [HttpDelete("roles/{roleId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteRoleFromUserAsync(Guid appId, Guid userId, Guid roleId)
        {
            var(app, user, role, result) = await VerifyAppOwnerUserRoleAsync(appId, userId, roleId, "delete role from user");
            if (result != null)
                return result;

            await userRoleRepository.DeleteByIdAsync(user.Id, role.Id);

            return NoContent();
        }

        [HttpPost("claims/{claimId:guid}")]
        [Authorize]
        public async Task<IActionResult> AddClaimToUserAsync(Guid appId, Guid userId, Guid claimId, [FromBody] ClaimValuePayload valuePayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var(app, user, claim, result) = await VerifyAppOwnerUserClaimAsync(appId, userId, claimId, "add claim to user");
            if (result != null)
                return result;

            var userClaim = new UserClaim(user.Id, claim.Id, valuePayload.Value);
            userClaim = await userClaimRepository.SaveAsync(userClaim);
            var claimValue = new ClaimValue(claim.Id, claim.Key, claim.Name, userClaim.Value);

            return Ok(mapper.Map<ClaimValueView>(claimValue));
        }

        [HttpDelete("claims/{claimId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteClaimFromUserAsync(Guid appId, Guid userId, Guid claimId)
        {
            var(app, user, claim, result) = await VerifyAppOwnerUserClaimAsync(appId, userId, claimId, "delete claim from user");
            if (result != null)
                return result;

            await userClaimRepository.DeleteByIdAsync(user.Id, claim.Id);

            return NoContent();
        }

        private async Task<ValueTuple<App, User, Role, IActionResult>> VerifyAppOwnerUserRoleAsync(Guid appId, Guid userId, Guid roleId, string operation)
        {
            var(app, user, result) = await VerifyAppOwnerUserAsync(appId, userId, operation);
            if (result != null)
                return (null, null, null, result);

            var role = await roleRepository.GetByIdAsync(roleId);
            if (role == null)
                return (null, null, null, NotFound("Role not found"));

            if (role.AppId != app.Id)
                return (null, null, null, Forbidden("Role belongs to another application"));

            return (app, user, role, null);
        }

        private async Task<ValueTuple<App, User, Claim, IActionResult>> VerifyAppOwnerUserClaimAsync(Guid appId, Guid userId, Guid claimId, string operation)
        {
            var(app, user, result) = await VerifyAppOwnerUserAsync(appId, userId, operation);
            if (result != null)
                return (null, null, null, result);

            var claim = await claimRepository.GetByIdAsync(claimId);
            if (claim == null)
                return (null, null, null, NotFound("Claim not found"));

            if (claim.AppId != app.Id)
                return (null, null, null, Forbidden("Claim belongs to another application"));

            return (app, user, claim, null);
        }

        private async Task<ValueTuple<App, User, IActionResult>> VerifyAppOwnerUserAsync(Guid appId, Guid userId, string operation)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return (null, null, NotFound("Application not found"));

            if (this.GetBaseId().UserId != app.OwnerId)
                return (null, null, Forbidden($"Need to be application owner to {operation}"));

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return (null, null, NotFound("User not found"));

            return (app, user, null);
        }
    }
}