using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Db.Repositories
{
    public interface ICompanyRoleClaimRepository
    {
        Task<CompanyRoleClaim> SaveAsync(CompanyRoleClaim claim);

        Task DeleteByIdAsync(Guid roleId, Guid claimId);
    }
}