using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Server.ViewModels.Requests.Companies;
using Server.ViewModels.Responses.Companies;
using Server.ViewModels.Responses.Users;

namespace Server.Host.Controllers;

[Route("companies")]
public class CompanyController : ServerController
{
    public CompanyController(IMediator mediator, IServiceProvider sp)
        : base(mediator, sp) { }

    [HttpPost]
    [Authorize]
    public Task<IResult<Guid>> RegisterCompanyAsync([FromBody] RegisterCompanyRequest request)
    {
        return HandleAsync<RegisterCompanyRequest, Guid>(request);
    }

    [HttpGet]
    [Authorize]
    public Task<IResult<IEnumerable<CompanyResponse>>> FindCompaniesAsync(string query)
    {
        var request = new FindCompaniesRequest { Query = query };

        return HandleAsync<FindCompaniesRequest, IEnumerable<CompanyResponse>>(request);
    }

    [HttpGet("my")]
    [Authorize]
    public Task<IResult<IEnumerable<CompanyResponse>>> ListMyCompaniesAsync()
    {
        return HandleAsync<ListMyCompaniesRequest, IEnumerable<CompanyResponse>>(new ListMyCompaniesRequest());
    }

    [HttpGet("{companyId:guid}")]
    [Authorize]
    public Task<IResult<CompanyResponse>> GetCompanyAsync(Guid companyId)
    {
        var request = new GetCompanyRequest { CompanyId = companyId };

        return HandleAsync<GetCompanyRequest, CompanyResponse>(request);
    }

    [HttpGet("{companyId:guid}/users")]
    [Authorize]
    public Task<IResult<IEnumerable<UserResponse>>> GetCompanyUsersAsync(Guid companyId)
    {
        var request = new GetCompanyUsersRequest { CompanyId = companyId };

        return HandleAsync<GetCompanyUsersRequest, IEnumerable<UserResponse>>(request);
    }

    [HttpPut("{companyId:guid}")]
    [Authorize]
    public Task<IResult> UpdateCompanyAsync(Guid companyId, [FromBody] UpdateCompanyRequestBody requestBody)
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
    public Task<IResult> SetCompanyOwnerAsync(Guid companyId, Guid userId)
    {
        var request = new SetCompanyOwnerRequest { CompanyId = companyId, UserId = userId };

        return HandleAsync(request);
    }

    [HttpDelete("{companyId:guid}")]
    [Authorize]
    public Task<IResult> UnregisterCompanyAsync(Guid companyId)
    {
        var request = new UnregisterCompanyRequest { CompanyId = companyId };

        return HandleAsync(request);
    }
}
