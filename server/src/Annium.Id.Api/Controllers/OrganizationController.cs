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
    [Route("organizations")]
    public class OrganizationController : LocalizedServerController
    {
        private readonly IOrganizationRepository organizationRepository;

        private readonly IUserRepository userRepository;

        public OrganizationController(
            IOrganizationRepository organizationRepository,
            IUserRepository userRepository,
            IStringLocalizer<OrganizationController> localizer
        ) : base(localizer)
        {
            this.organizationRepository = organizationRepository;
            this.userRepository = userRepository;
        }

        [HttpPut]
        [AuthorizeId]
        public async Task<IActionResult> RegisterOrganizationAsync([FromBody] OrganizationPayload organizationPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (organizationPayload.ParentId != null &&
                (await organizationRepository.GetByIdAsync(organizationPayload.ParentId.Value)) == null)
                return NotFound($"Parent organization not found");

            if ((await organizationRepository.FindByKeyAsync(organizationPayload.Key)) != null)
                return Conflict($"Organization key {organizationPayload.Key} is already used");

            var organization = new Organization(
                this.GetId().UserId,
                organizationPayload.ParentId,
                organizationPayload.Key,
                organizationPayload.Name
            );

            organization = await organizationRepository.CreateAsync(organization);

            return Ok(new OrganizationView(organization));
        }

        [HttpGet("{organizationId:guid}")]
        public async Task<IActionResult> GetOrganizationInfoAsync(Guid organizationId)
        {
            var organization = await organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
                return NotFound();

            return Ok(new OrganizationView(organization));
        }

        [HttpPost("{organizationId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> UpdateOrganizationAsync(Guid organizationId, [FromBody] OrganizationPayload organizationPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var organization = await organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
                return NotFound();

            if (this.GetId().UserId != organization.OwnerId)
                return Forbidden("Need to be owner to update organization");

            if (organizationPayload.ParentId != null &&
                organizationPayload.ParentId != organization.ParentId &&
                (await organizationRepository.GetByIdAsync(organizationPayload.ParentId.Value)) == null)
                return NotFound($"Parent organization not found");

            if (organizationPayload.Key != organization.Key &&
                (await organizationRepository.FindByKeyAsync(organizationPayload.Key)) != null)
                return Conflict($"Organization key {organizationPayload.Key} is already used");

            organization.ParentId = organizationPayload.ParentId;
            organization.Key = organizationPayload.Key;
            organization.Name = organizationPayload.Name;

            organization = await organizationRepository.UpdateAsync(organization);

            return Ok(new OrganizationView(organization));
        }

        [HttpPost("{organizationId:guid}/owner/{userId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> SetOrganizationOwnerAsync(Guid organizationId, Guid userId)
        {
            var organization = await organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
                return NotFound();

            if (this.GetId().UserId != organization.OwnerId)
                return Forbidden("Need to be owner to change organization owner");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            organization.OwnerId = user.Id;

            organization = await organizationRepository.UpdateAsync(organization);

            return Ok(new OrganizationView(organization));
        }

        [HttpDelete("{organizationId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> UnregisterOrganizationAsync(Guid organizationId)
        {
            var organization = await organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
                return NotFound();

            if (this.GetId().UserId != organization.OwnerId)
                return Forbidden("Need to be owner to delete organization");

            await organizationRepository.DeleteByIdAsync(organization.Id);

            return NoContent();
        }
    }
}