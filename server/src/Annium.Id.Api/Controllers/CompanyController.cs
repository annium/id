using System;
using System.Linq;
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
    [Route("companies")]
    public class CompanyController : LocalizedServerController
    {
        private readonly ICompanyRepository companyRepository;

        private readonly IUserRepository userRepository;

        private readonly ICompanyUserRepository companyUserRepository;

        public CompanyController(
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            ICompanyUserRepository companyUserRepository,
            IStringLocalizer<CompanyController> localizer
        ) : base(localizer)
        {
            this.companyRepository = companyRepository;
            this.userRepository = userRepository;
            this.companyUserRepository = companyUserRepository;
        }

        [HttpPut]
        [AuthorizeId]
        public async Task<IActionResult> RegisterCompanyAsync([FromBody] CompanyPayload companyPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (companyPayload.ParentId != null &&
                (await companyRepository.GetByIdAsync(companyPayload.ParentId.Value)) == null)
                return NotFound($"Parent company not found");

            if ((await companyRepository.FindByKeyAsync(companyPayload.Key)) != null)
                return Conflict($"Company key {companyPayload.Key} is already used");

            var company = new Company(
                this.GetId().UserId,
                companyPayload.ParentId,
                companyPayload.Key,
                companyPayload.Name
            );

            company = await companyRepository.CreateAsync(company);

            return Ok(new CompanyView(company));
        }

        [HttpGet("{companyId:guid}")]
        public async Task<IActionResult> GetCompanyInfoAsync(Guid companyId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return NotFound();

            return Ok(new CompanyView(company));
        }

        [HttpGet("{companyId:guid}/users")]
        public async Task<IActionResult> GetCompanyUsersAsync(Guid companyId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return NotFound();

            var users = await companyUserRepository.GetAllAsync(companyId);

            return Ok(users.Select(u => new UserView(u)).ToArray());
        }

        [HttpPost("{companyId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> UpdateCompanyAsync(Guid companyId, [FromBody] CompanyPayload companyPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return NotFound();

            if (this.GetId().UserId != company.OwnerId)
                return Forbidden("Need to be owner to update company");

            if (companyPayload.ParentId != null &&
                companyPayload.ParentId != company.ParentId &&
                (await companyRepository.GetByIdAsync(companyPayload.ParentId.Value)) == null)
                return NotFound($"Parent company not found");

            if (companyPayload.Key != company.Key &&
                (await companyRepository.FindByKeyAsync(companyPayload.Key)) != null)
                return Conflict($"Company key {companyPayload.Key} is already used");

            company.ParentId = companyPayload.ParentId;
            company.Key = companyPayload.Key;
            company.Name = companyPayload.Name;

            company = await companyRepository.UpdateAsync(company);

            return Ok(new CompanyView(company));
        }

        [HttpPost("{companyId:guid}/owner/{userId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> SetCompanyOwnerAsync(Guid companyId, Guid userId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return NotFound();

            if (this.GetId().UserId != company.OwnerId)
                return Forbidden("Need to be owner to change company owner");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            company.OwnerId = user.Id;

            company = await companyRepository.UpdateAsync(company);

            return Ok(new CompanyView(company));
        }

        [HttpDelete("{companyId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> UnregisterCompanyAsync(Guid companyId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return NotFound();

            if (this.GetId().UserId != company.OwnerId)
                return Forbidden("Need to be owner to delete company");

            await companyRepository.DeleteByIdAsync(company.Id);

            return NoContent();
        }
    }
}