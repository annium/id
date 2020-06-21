using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Repositories
{
    public interface IRoleClaimRepository
    {
        Task<RoleClaim> SaveAsync(RoleClaim claim);
        Task DeleteByIdAsync(Guid roleId, Guid claimId);
    }
}