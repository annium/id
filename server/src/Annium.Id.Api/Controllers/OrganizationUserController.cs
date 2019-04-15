using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Id.AspNetCore;
using Annium.Id.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Annium.Id.Api.Controllers
{
    [Route("organizations/{organizationId:guid}/users/{userId:guid}")]
    public class OrganizationUserController : LocalizedServerController
    {
        private readonly IOrganizationRepository organizationRepository;

        private readonly IUserRepository userRepository;

        public OrganizationUserController(
            IOrganizationRepository organizationRepository,
            IUserRepository userRepository,
            IStringLocalizer<OrganizationUserController> localizer
        ) : base(localizer)
        {
            this.organizationRepository = organizationRepository;
            this.userRepository = userRepository;
        }

        [HttpPut]
        [AuthorizeId]
        public async Task<IActionResult> AddUserToOrganizationAsync(Guid organizationId, Guid userId)
        {
            var organization = await organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
                return NotFound();

            if (this.GetId().UserId != organization.OwnerId)
                return Forbidden("Need to be application owner to add user to organization");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();


            return NoContent();
        }

        [HttpPut("roles/{roleId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> AddRoleToOrganizationUserAsync(Guid organizationId, Guid userId)
        {
            return NoContent();
        }

        [HttpDelete("roles/{roleId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteRoleFromOrganizationUserAsync(Guid organizationId, Guid userId)
        {
            return NoContent();
        }

        [HttpPut("claims/{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> AddClaimToOrganizationUserAsync(Guid organizationId, Guid userId)
        {
            return NoContent();
        }

        [HttpDelete("claims/{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteClaimFromOrganizationUserAsync(Guid organizationId, Guid userId)
        {
            return NoContent();
        }

        [HttpDelete]
        [AuthorizeId]
        public async Task<IActionResult> DeleteUserFromOrganizationAsync(Guid organizationId, Guid userId)
        {
            return NoContent();
        }
    }
}