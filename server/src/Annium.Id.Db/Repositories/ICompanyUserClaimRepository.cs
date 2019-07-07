using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Annium.Id.Db.Repositories
{
    public interface ICompanyUserClaimRepository
    {
        Task<CompanyUserClaim> SaveAsync(CompanyUserClaim claim);

        Task<IReadOnlyDictionary<Guid, ClaimValue[]>> GetCompaniesUserClaimsAsync(Guid appId, Guid userId);

        Task DeleteByIdAsync(Guid companyId, Guid userId, Guid claimId);
    }
}