using System;
using System.Threading.Tasks;

namespace Annium.Id.Db
{
    public interface ICompanyUserClaimRepository
    {
        Task<CompanyUserClaim> SaveAsync(CompanyUserClaim claim);

        Task DeleteByIdAsync(Guid companyId, Guid userId, Guid claimId);
    }
}