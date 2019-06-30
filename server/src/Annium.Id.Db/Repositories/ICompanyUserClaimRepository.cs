using System;
using System.Threading.Tasks;

namespace Annium.Id.Db.Repositories
{
    public interface ICompanyUserClaimRepository
    {
        Task<CompanyUserClaim> SaveAsync(CompanyUserClaim claim);

        Task DeleteByIdAsync(Guid companyId, Guid userId, Guid claimId);
    }
}