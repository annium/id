using System;
using System.Threading.Tasks;

namespace Annium.Id.Db.Repositories
{
    public interface IUserClaimRepository
    {
        Task<UserClaim> SaveAsync(UserClaim claim);

        Task DeleteByIdAsync(Guid userId, Guid claimId);
    }
}