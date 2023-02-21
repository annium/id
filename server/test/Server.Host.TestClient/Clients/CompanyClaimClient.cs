using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyClaims;
using Server.ViewModels.Responses.CompanyClaims;

namespace Server.Host.TestClient.Clients;

public class CompanyClaimClient : ClientBase
{
    public CompanyClaimClient(IHttpRequest request) : base(request)
    {
    }

    public async Task<IHttpResponse<IResult<Guid>>> CreateCompanyClaim(
        CreateCompanyClaimRequest body
    )
    {
        return await Request.Clone()
            .Post("companies/claims")
            .JsonContent(body)
            .AsResponseAsync<IResult<Guid>>();
    }

    public async Task<IHttpResponse<IResult<CompanyClaimResponse[]>>> ListCompanyClaims(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get("companies/claims")
            .Param("appId", appId)
            .AsResponseAsync<IResult<CompanyClaimResponse[]>>();
    }

    public async Task<IHttpResponse<IResult>> UpdateCompanyClaim(
        Guid claimId,
        UpdateCompanyClaimRequestBody body
    )
    {
        return await Request.Clone()
            .Put($"companies/claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteCompanyClaim(
        Guid claimId
    )
    {
        return await Request.Clone()
            .Delete($"companies/claims/{claimId}")
            .AsResponseAsync<IResult>();
    }
}