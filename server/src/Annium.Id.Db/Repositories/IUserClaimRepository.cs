using System;
using System.Threading.Tasks;

namespace Annium.Id.Db.Repositories
{
    public interface IUserClaimRepository
    {
        Task<UserClaim> SaveAsync(UserClaim claim);

        Task<ClaimValue[]> GetUserClaimsAsync(Guid appId, Guid userId);

        Task DeleteByIdAsync(Guid userId, Guid claimId);
    }
}