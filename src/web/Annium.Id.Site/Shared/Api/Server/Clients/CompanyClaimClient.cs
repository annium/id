using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.CompanyClaims;
using Annium.Id.Api.ViewModels.Responses.CompanyClaims;
using Annium.Net.Http;

namespace Annium.Id.Site.Shared.Api.Server.Clients
{
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
                .AsAsync(Result.New(default(Guid)).Error("Request failed"));
        }

        public async Task<IResult<IEnumerable<CompanyClaimResponse>>> ListCompanyClaims(
            Guid appId
        )
        {
            return await Request.Clone()
                .Get("companies/claims")
                .Param("appId", appId)
                .AsAsync(Result.New<IEnumerable<CompanyClaimResponse>>(Array.Empty<CompanyClaimResponse>()).Error("Request failed"));
        }

        public async Task<IResult> UpdateCompanyClaim(
            Guid claimId,
            UpdateCompanyClaimRequestBody body
        )
        {
            return await Request.Clone()
                .Put($"companies/claims/{claimId}")
                .JsonContent(body)
                .AsAsync(Result.New().Error("Request failed"));
        }

        public async Task<IResult> DeleteCompanyClaim(
            Guid claimId
        )
        {
            return await Request.Clone()
                .Delete($"companies/claims/{claimId}")
                .AsAsync(Result.New().Error("Request failed"));
        }
    }
}