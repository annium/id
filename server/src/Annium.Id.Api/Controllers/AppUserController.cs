using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Id.Api.Payloads;
using Annium.Id.AspNetCore;
using Annium.Id.Db;
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

        public AppUserController(
            IAppRepository appRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IClaimRepository claimRepository,
            IUserRoleRepository userRoleRepository,
            IUserClaimRepository userClaimRepository,
            IStringLocalizer<AppUserController> localizer
        ) : base(localizer)
        {
            this.appRepository = appRepository;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.claimRepository = claimRepository;
            this.userRoleRepository = userRoleRepository;
            this.userClaimRepository = userClaimRepository;
        }

        [HttpPut("roles/{roleId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> AddRoleToUserRAsync(Guid appId, Guid userId, Guid roleId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be application owner to add role to user");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            var role = await roleRepository.GetByIdAsync(roleId);
            if (role == null)
                return NotFound();

            if (role.AppId != app.Id)
                return Forbidden("Role belongs to another application");

            var userRole = new UserRole(user.Id, role.Id);

            userRole = await userRoleRepository.SaveAsync(userRole);

            return NoContent();
        }

        [HttpDelete("roles/{roleId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteRoleFromUserAsync(Guid appId, Guid userId, Guid roleId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be application owner to delete role from user");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            var role = await roleRepository.GetByIdAsync(roleId);
            if (role == null)
                return NotFound();

            if (role.AppId != app.Id)
                return Forbidden("Role belongs to another application");

            await userRoleRepository.DeleteByIdAsync(user.Id, role.Id);

            return NoContent();
        }

        [HttpPost("claims/{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> AddClaimToUserRAsync(Guid appId, Guid userId, Guid claimId, [FromBody] ClaimValuePayload valuePayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be application owner to add claim to user");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            var claim = await claimRepository.GetByIdAsync(claimId);
            if (claim == null)
                return NotFound();

            if (claim.AppId != app.Id)
                return Forbidden("Claim belongs to another application");

            var userClaim = new UserClaim(user.Id, claim.Id, valuePayload.Value);

            userClaim = await userClaimRepository.SaveAsync(userClaim);

            return Ok(new ClaimValue(claim.Id, claim.Key, claim.Name, userClaim.Value));
        }

        [HttpDelete("claims/{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteClaimFromUserAsync(Guid appId, Guid userId, Guid claimId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be application owner to delete claim from user");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            var claim = await claimRepository.GetByIdAsync(claimId);
            if (claim == null)
                return NotFound();

            if (claim.AppId != app.Id)
                return Forbidden("Claim belongs to another application");

            await userClaimRepository.DeleteByIdAsync(user.Id, claim.Id);

            return NoContent();
        }
    }
}