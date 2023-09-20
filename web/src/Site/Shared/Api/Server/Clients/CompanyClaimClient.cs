using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyClaims;
using Server.ViewModels.Responses.CompanyClaims;

namespace Site.Shared.Api.Server.Clients;

public class CompanyClaimClient
{
    private readonly IHttpRequest _request;

    internal CompanyClaimClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IResult<Guid>> CreateCompanyClaim(
        CreateCompanyClaimRequest body
    )
    {
        return await _request
            .Post("companies/claims")
            .JsonContent(body)
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult<CompanyClaimResponse[]>> ListCompanyClaims(
        Guid appId
    )
    {
        return await _request
            .Get("companies/claims")
            .Param("appId", appId)
            .AsAsync<IResult<CompanyClaimResponse[]>>();
    }

    public async Task<IResult> UpdateCompanyClaim(
        Guid claimId,
        UpdateCompanyClaimRequestBody body
    )
    {
        return await _request
            .Put($"companies/claims/{claimId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteCompanyClaim(
        Guid claimId
    )
    {
        return await _request
            .Delete($"companies/claims/{claimId}")
            .AsAsync<IResult>();
    }
}