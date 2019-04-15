using System;
using System.Threading.Tasks;

namespace Annium.Id.Db
{
    public interface ICompanyRoleClaimRepository
    {
        Task<CompanyRoleClaim> SaveAsync(CompanyRoleClaim claim);

        Task DeleteByIdAsync(Guid roleId, Guid claimId);
    }
}