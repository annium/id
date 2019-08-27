using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Id.Api.Payloads;
using Annium.Id.AspNetCore;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;
using Annium.Localization.Abstractions;
using Microsoft.AspNetCore.Mvc;

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
            ILocalizer<CompanyUserController> localizer
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddUserToCompanyAsync(Guid companyId, Guid userId)
        {
            var(company, user, result) = await VerifyCompanyOwnerUserAsync(companyId, userId, "add user to company");
            if (result != null)
                return result;

            await companyUserRepository.SaveAsync(new CompanyUser(company.Id, user.Id));

            return NoContent();
        }

        [HttpPost("roles/{roleId:guid}")]
        [Authorize]
        public async Task<IActionResult> AddCompanyRoleToCompanyUserAsync(Guid companyId, Guid userId, Guid roleId)
        {
            var(company, user, role, result) = await VerifyCompanyOwnerMemberRoleAsync(companyId, userId, roleId, "add role to company member");
            if (result != null)
                return result;

            await companyUserRoleRepository.SaveAsync(new CompanyUserRole(company.Id, user.Id, role.Id));

            return NoContent();
        }

        [HttpDelete("roles/{roleId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteCompanyRoleFromCompanyUserAsync(Guid companyId, Guid userId, Guid roleId)
        {
            var(company, user, role, result) = await VerifyCompanyOwnerMemberRoleAsync(companyId, userId, roleId, "delete role from company member");
            if (result != null)
                return result;

            await companyUserRoleRepository.DeleteByIdAsync(company.Id, user.Id, role.Id);

            return NoContent();
        }

        [HttpPost("claims/{claimId:guid}")]
        [Authorize]
        public async Task<IActionResult> AddCompanyClaimToCompanyUserAsync(Guid companyId, Guid userId, Guid claimId, [FromBody] ClaimValuePayload claimValuePayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var(company, user, claim, result) = await VerifyCompanyOwnerMemberClaimAsync(companyId, userId, claimId, "add claim to company member");
            if (result != null)
                return result;

            await companyUserClaimRepository.SaveAsync(new CompanyUserClaim(company.Id, user.Id, claim.Id, claimValuePayload.Value));

            return NoContent();
        }

        [HttpDelete("claims/{claimId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteCompanyClaimFromCompanyUserAsync(Guid companyId, Guid userId, Guid claimId)
        {
            var(company, user, claim, result) = await VerifyCompanyOwnerMemberClaimAsync(companyId, userId, claimId, "delete claim from company member");
            if (result != null)
                return result;

            await companyUserClaimRepository.DeleteByIdAsync(company.Id, user.Id, claim.Id);

            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteUserFromCompanyAsync(Guid companyId, Guid userId)
        {
            var(company, user, result) = await VerifyCompanyOwnerUserAsync(companyId, userId, "delete user from company");
            if (result != null)
                return result;

            // TODO: remove all user roles and claims
            await companyUserRepository.DeleteByIdAsync(company.Id, user.Id);

            return NoContent();
        }

        private async Task<ValueTuple<Company, User, CompanyRole, IActionResult>> VerifyCompanyOwnerMemberRoleAsync(Guid companyId, Guid userId, Guid roleId, string operation)
        {
            var(company, user, result) = await VerifyCompanyOwnerMemberAsync(companyId, userId, operation);
            if (result != null)
                return (null, null, null, result);

            var role = await companyRoleRepository.GetByIdAsync(roleId);
            if (role == null)
                return (null, null, null, NotFound("Role not found"));

            return (company, user, role, null);
        }

        private async Task<ValueTuple<Company, User, CompanyClaim, IActionResult>> VerifyCompanyOwnerMemberClaimAsync(Guid companyId, Guid userId, Guid claimId, string operation)
        {
            var(company, user, result) = await VerifyCompanyOwnerMemberAsync(companyId, userId, operation);
            if (result != null)
                return (null, null, null, result);

            var claim = await companyClaimRepository.GetByIdAsync(claimId);
            if (claim == null)
                return (null, null, null, NotFound("Claim not found"));

            return (company, user, claim, null);
        }

        private async Task<ValueTuple<Company, User, IActionResult>> VerifyCompanyOwnerMemberAsync(Guid companyId, Guid userId, string operation)
        {
            var(company, user, result) = await VerifyCompanyOwnerUserAsync(companyId, userId, operation);
            if (result != null)
                return (null, null, result);

            if ((await companyUserRepository.GetByIdAsync(company.Id, user.Id)) == null)
                return (null, null, Forbidden("User is not company member"));

            return (company, user, null);
        }

        private async Task<ValueTuple<Company, User, IActionResult>> VerifyCompanyOwnerUserAsync(Guid companyId, Guid userId, string operation)
        {
            var(company, result) = await VerifyCompanyOwnerAsync(companyId, operation);
            if (result != null)
                return (null, null, result);

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return (null, null, NotFound("User not found"));

            return (company, user, null);
        }

        private async Task<ValueTuple<Company, IActionResult>> VerifyCompanyOwnerAsync(Guid companyId, string operation)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return (null, NotFound("Company not found"));

            if (this.GetBaseId().UserId != company.OwnerId)
                return (null, Forbidden($"Need to be company owner to {operation}"));

            return (company, null);
        }
    }
}