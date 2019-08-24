using System;
using System.Linq;
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
    [Route("companies")]
    public class CompanyController : LocalizedServerController
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IUserRepository userRepository;
        private readonly ICompanyUserRepository companyUserRepository;
        private readonly IMapper mapper;

        public CompanyController(
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            ICompanyUserRepository companyUserRepository,
            IMapper mapper,
            IStringLocalizer<CompanyController> localizer
        ) : base(localizer)
        {
            this.companyRepository = companyRepository;
            this.userRepository = userRepository;
            this.companyUserRepository = companyUserRepository;
            this.mapper = mapper;
        }

        [HttpPut]
        [Authorize]
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
                this.GetBaseId().UserId,
                companyPayload.ParentId,
                companyPayload.Key,
                companyPayload.Name
            );

            company = await companyRepository.CreateAsync(company);

            return Ok(mapper.Map<CompanyPrivateView>(company));
        }

        [HttpGet("{companyId:guid}")]
        public async Task<IActionResult> GetCompanyInfoAsync(Guid companyId)
        {
            var(company, result) = await VerifyCompanyAsync(companyId);
            if (result != null)
                return result;

            return Ok(mapper.Map<CompanyPublicView>(company));
        }

        [HttpGet("{companyId:guid}/users")]
        public async Task<IActionResult> GetCompanyUsersAsync(Guid companyId)
        {
            var(company, result) = await VerifyCompanyAsync(companyId);
            if (result != null)
                return result;

            var users = await companyUserRepository.GetAllAsync(companyId);

            return Ok(users.Select(mapper.Map<UserPublicView>).ToArray());
        }

        [HttpPost("{companyId:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateCompanyAsync(Guid companyId, [FromBody] CompanyPayload companyPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var(company, result) = await VerifyCompanyOwnerAsync(companyId, "update company");
            if (result != null)
                return result;

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

            return Ok(mapper.Map<CompanyPrivateView>(company));
        }

        [HttpPost("{companyId:guid}/owner/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> SetCompanyOwnerAsync(Guid companyId, Guid userId)
        {
            var(company, result) = await VerifyCompanyOwnerAsync(companyId, "change company owner");
            if (result != null)
                return result;

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            company.OwnerId = user.Id;

            company = await companyRepository.UpdateAsync(company);

            return Ok(mapper.Map<CompanyPrivateView>(company));
        }

        [HttpDelete("{companyId:guid}")]
        [Authorize]
        public async Task<IActionResult> UnregisterCompanyAsync(Guid companyId)
        {
            var(company, result) = await VerifyCompanyOwnerAsync(companyId, "delete company");
            if (result != null)
                return result;

            await companyRepository.DeleteByIdAsync(company.Id);

            return NoContent();
        }

        private async Task<ValueTuple<Company, IActionResult>> VerifyCompanyOwnerAsync(Guid companyId, string operation)
        {
            var(company, result) = await VerifyCompanyAsync(companyId);
            if (result != null)
                return (null, result);

            if (this.GetBaseId().UserId != company.OwnerId)
                return (null, Forbidden($"Need to be company owner to {operation}"));

            return (company, null);
        }

        private async Task<ValueTuple<Company, IActionResult>> VerifyCompanyAsync(Guid companyId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if (company == null)
                return (null, NotFound("Company not found"));

            return (company, null);
        }
    }
}