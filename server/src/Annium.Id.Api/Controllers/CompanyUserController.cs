using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
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

        public CompanyUserController(
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            IStringLocalizer<CompanyUserController> localizer
        ) : base(localizer)
        {
            this.companyRepository = companyRepository;
            this.userRepository = userRepository;
        }

        [HttpPut]
        [AuthorizeId]
        public async Task<IActionResult> AddUserToCompanyAsync(Guid companyId, Guid userId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return NotFound();

            if (this.GetId().UserId != company.OwnerId)
                return Forbidden("Need to be application owner to add user to company");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();


            return NoContent();
        }

        [HttpPut("roles/{roleId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> AddRoleToCompanyUserAsync(Guid companyId, Guid userId)
        {
            return NoContent();
        }

        [HttpDelete("roles/{roleId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteRoleFromCompanyUserAsync(Guid companyId, Guid userId)
        {
            return NoContent();
        }

        [HttpPut("claims/{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> AddClaimToCompanyUserAsync(Guid companyId, Guid userId)
        {
            return NoContent();
        }

        [HttpDelete("claims/{claimId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteClaimFromCompanyUserAsync(Guid companyId, Guid userId)
        {
            return NoContent();
        }

        [HttpDelete]
        [AuthorizeId]
        public async Task<IActionResult> DeleteUserFromCompanyAsync(Guid companyId, Guid userId)
        {
            return NoContent();
        }
    }
}