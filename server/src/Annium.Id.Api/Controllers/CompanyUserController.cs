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
    [Route("companies/{companyId:guid}/users/{userId:guid}")]
    public class CompanyUserController : LocalizedServerController
    {
        private readonly ICompanyRepository companyRepository;

        private readonly IUserRepository userRepository;

        private readonly ICompanyRoleRepository companyRoleRepository;

        private readonly ICompanyClaimRepository companyClaimRepository;

        private readonly ICompanyUserRepository companyUserRepository;

        private readonly ICompanyUserRoleRepository companyUserRoleRepository;

        private readonly ICompanyUserClaimRepository companyUserClaimRepository;

        public CompanyUserController(
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            ICompanyRoleRepository companyRoleRepository,
            ICompanyClaimRepository companyClaimRepository,
            ICompanyUserRepository companyUserRepository,
            ICompanyUserRoleRepository companyUserRoleRepository,
            ICompanyUserClaimRepository companyUserClaimRepository,
            IStringLocalizer<CompanyUserController> localizer
        ) : base(localizer)
        {
            this.companyRepository = companyRepository;
            this.userRepository = userRepository;
            this.companyRoleRepository = companyRoleRepository;
            this.companyClaimRepository = companyClaimRepository;
            this.companyUserRepository = companyUserRepository;
            this.companyUserRoleRepository = companyUserRoleRepository;
            this.companyUserClaimRepository = companyUserClaimRepository;
        }

        [HttpPut]
        [AuthorizeId]
        public async Task<IActionResult> AddUserToCompanyAsync(Guid companyId, Guid userId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return NotFound();

            if (this.GetId().UserId != company.OwnerId)
                return Forbidden("Need to be company owner to add user to company");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            await companyUserRepository.SaveAsync(new CompanyUser(company.Id, user.Id));

            return NoContent();
        }

        [HttpPost("roles/{roleId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> AddRoleToCompanyUserAsync(Guid companyId, Guid userId, Guid roleId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return NotFound();

            if (this.GetId().UserId != company.OwnerId)
                return Forbidden("Need to be company owner to add role to company member");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            if ((await companyUserRepository.GetByIdAsync(company.Id, user.Id)) == null)
                return Forbidden("User is not company member");

            var role = await companyRoleRepository.GetByIdAsync(roleId);
            if (role == null)
                return NotFound();

            await companyUserRoleRepository.SaveAsync(new CompanyUserRole(company.Id, user.Id, role.Id));

            return NoContent();
        }

        [HttpDelete("roles/{roleId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteRoleFromCompanyUserAsync(Guid companyId, Guid userId, Guid roleId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return NotFound();

            if (this.GetId().UserId != company.OwnerId)
                return Forbidden("Need to be company owner to delete role from company member");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            if ((await companyUserRepository.GetByIdAsync(company.Id, user.Id)) == null)
                return Forbidden("User is not company member");

            var role = await companyRoleRepository.GetByIdAsync(roleId);
            if (role == null)
                return NotFound();

            await companyUserRoleRepository.DeleteByIdAsync(company.Id, user.Id, role.Id);

            return NoContent();
        }

        [HttpPost("claims/{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> AddClaimToCompanyUserAsync(Guid companyId, Guid userId, Guid claimId, [FromBody] ClaimValuePayload claimValuePayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return NotFound();

            if (this.GetId().UserId != company.OwnerId)
                return Forbidden("Need to be company owner to add claim to company member");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            if ((await companyUserRepository.GetByIdAsync(company.Id, user.Id)) == null)
                return Forbidden("User is not company member");

            var claim = await companyClaimRepository.GetByIdAsync(claimId);
            if (claim == null)
                return NotFound();

            await companyUserClaimRepository.SaveAsync(new CompanyUserClaim(company.Id, user.Id, claim.Id, claimValuePayload.Value));

            return NoContent();
        }

        [HttpDelete("claims/{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteClaimFromCompanyUserAsync(Guid companyId, Guid userId, Guid claimId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return NotFound();

            if (this.GetId().UserId != company.OwnerId)
                return Forbidden("Need to be company owner to dele claim from company member");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            if ((await companyUserRepository.GetByIdAsync(company.Id, user.Id)) == null)
                return Forbidden("User is not company member");

            var claim = await companyClaimRepository.GetByIdAsync(claimId);
            if (claim == null)
                return NotFound();

            await companyUserClaimRepository.DeleteByIdAsync(company.Id, user.Id, claim.Id);

            return NoContent();
        }

        [HttpDelete]
        [AuthorizeId]
        public async Task<IActionResult> DeleteUserFromCompanyAsync(Guid companyId, Guid userId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return NotFound();

            if (this.GetId().UserId != company.OwnerId)
                return Forbidden("Need to be company owner to delete user from company");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            await companyUserRepository.DeleteByIdAsync(company.Id, user.Id);

            return NoContent();
        }
    }
}