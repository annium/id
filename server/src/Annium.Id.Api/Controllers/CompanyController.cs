using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.AspNetCore;
using Annium.Id.ViewModels.Companies.Requests;
using Annium.Id.ViewModels.Companies.Responses;
using Annium.Id.ViewModels.Users.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("companies")]
    public class CompanyController : ServerController
    {
        public CompanyController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost]
        [Authorize]
        public Task<IActionResult> RegisterCompanyAsync([FromBody] RegisterCompanyRequest request)
        {
            return HandleAsync<RegisterCompanyRequest, Guid>(request);
        }

        [HttpGet("{companyId:guid}")]
        public Task<IActionResult> GetCompanyInfoAsync(Guid companyId)
        {
            var request = new GetCompanyRequest { CompanyId = companyId };

            return HandleAsync<GetCompanyRequest, CompanyResponse>(request);
        }

        [HttpGet("{companyId:guid}/users")]
        public Task<IActionResult> GetCompanyUsersAsync(Guid companyId)
        {
            var request = new GetCompanyUsersRequest { CompanyId = companyId };

            return HandleAsync<GetCompanyUsersRequest, IEnumerable<UserResponse>>(request);
        }

        [HttpPut("{companyId:guid}")]
        [Authorize]
        public Task<IActionResult> UpdateCompanyAsync(Guid companyId, [FromBody] UpdateCompanyRequestBase requestBase)
        {
            var request = new UpdateCompanyRequest
            {
                CompanyId = companyId,
                ParentId = requestBase.ParentId,
                Key = requestBase.Key,
                Name = requestBase.Name,
            };

            return HandleAsync(request);
        }

        [HttpPut("{companyId:guid}/owner/{userId:guid}")]
        [Authorize]
        public Task<IActionResult> SetCompanyOwnerAsync(Guid companyId, Guid userId)
        {
            var request = new SetCompanyOwnerRequest { CompanyId = companyId, UserId = userId };

            return HandleAsync(request);
        }

        [HttpDelete("{companyId:guid}")]
        [Authorize]
        public Task<IActionResult> UnregisterCompanyAsync(Guid companyId)
        {
            var request = new UnregisterCompanyRequest { CompanyId = companyId };

            return HandleAsync(request);
        }
    }
}