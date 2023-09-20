using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyClaims;
using Server.ViewModels.Responses.CompanyClaims;

namespace Server.Host.TestClient.Clients;

public class CompanyClaimClient
{
    private readonly IHttpRequest _request;

    internal CompanyClaimClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IHttpResponse<IResult<Guid>>> CreateCompanyClaim(
        CreateCompanyClaimRequest body
    )
    {
        return await _request
            .Post("companies/claims")
            .JsonContent(body)
            .AsResponseAsync<IResult<Guid>>();
    }

    public async Task<IHttpResponse<IResult<CompanyClaimResponse[]>>> ListCompanyClaims(
        Guid appId
    )
    {
        return await _request
            .Get("companies/claims")
            .Param("appId", appId)
            .AsResponseAsync<IResult<CompanyClaimResponse[]>>();
    }

    public async Task<IHttpResponse<IResult>> UpdateCompanyClaim(
        Guid claimId,
        UpdateCompanyClaimRequestBody body
    )
    {
        return await _request
            .Put($"companies/claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteCompanyClaim(
        Guid claimId
    )
    {
        return await _request
            .Delete($"companies/claims/{claimId}")
            .AsResponseAsync<IResult>();
    }
}