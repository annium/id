using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyClaims;
using Server.ViewModels.Responses.CompanyClaims;

namespace Site.Shared.Api.Server.Clients;

public class CompanyClaimClient : ClientBase
{
    public CompanyClaimClient(IHttpRequest request) : base(request)
    {
    }

    public async Task<IResult<Guid>> CreateCompanyClaim(
        CreateCompanyClaimRequest body
    )
    {
        return await Request.Clone()
            .Post("companies/claims")
            .JsonContent(body)
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult<CompanyClaimResponse[]>> ListCompanyClaims(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get("companies/claims")
            .Param("appId", appId)
            .AsAsync<IResult<CompanyClaimResponse[]>>();
    }

    public async Task<IResult> UpdateCompanyClaim(
        Guid claimId,
        UpdateCompanyClaimRequestBody body
    )
    {
        return await Request.Clone()
            .Put($"companies/claims/{claimId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteCompanyClaim(
        Guid claimId
    )
    {
        return await Request.Clone()
            .Delete($"companies/claims/{claimId}")
            .AsAsync<IResult>();
    }
}