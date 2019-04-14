using System;
using System.Threading.Tasks;

namespace Annium.Id.Db
{
    public interface IRoleClaimRepository
    {
        Task<RoleClaim> SaveAsync(RoleClaim claim);

        Task DeleteByIdAsync(Guid roleId, Guid claimId);
    }
}