using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.AspNetCore;
using Annium.Id.Api.ViewModels.Companies.Requests;
using Annium.Id.Api.ViewModels.Companies.Responses;
using Annium.Id.Api.ViewModels.Users.Responses;
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
        public Task<IResult<Guid>> RegisterCompany([FromBody] RegisterCompanyRequest request)
        {
            return HandleAsync<RegisterCompanyRequest, Guid>(request);
        }

        [HttpGet("{companyId:guid}")]
        public Task<IResult<CompanyResponse>> GetCompanyInfo(Guid companyId)
        {
            var request = new GetCompanyRequest { CompanyId = companyId };

            return HandleAsync<GetCompanyRequest, CompanyResponse>(request);
        }

        [HttpGet("{companyId:guid}/users")]
        public Task<IResult<IEnumerable<UserResponse>>> GetCompanyUsers(Guid companyId)
        {
            var request = new GetCompanyUsersRequest { CompanyId = companyId };

            return HandleAsync<GetCompanyUsersRequest, IEnumerable<UserResponse>>(request);
        }

        [HttpPut("{companyId:guid}")]
        [Authorize]
        public Task<IResult> UpdateCompany(Guid companyId, [FromBody] UpdateCompanyRequestBody requestBody)
        {
            var request = new UpdateCompanyRequest
            {
                CompanyId = companyId,
                ParentId = requestBody.ParentId,
                Name = requestBody.Name,
            };

            return HandleAsync(request);
        }

        [HttpPut("{companyId:guid}/owner/{userId:guid}")]
        [Authorize]
        public Task<IResult> SetCompanyOwner(Guid companyId, Guid userId)
        {
            var request = new SetCompanyOwnerRequest { CompanyId = companyId, UserId = userId };

            return HandleAsync(request);
        }

        [HttpDelete("{companyId:guid}")]
        [Authorize]
        public Task<IResult> UnregisterCompany(Guid companyId)
        {
            var request = new UnregisterCompanyRequest { CompanyId = companyId };

            return HandleAsync(request);
        }
    }
}