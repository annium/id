using System;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface IUserClaimRepository
{
    Task<UserClaim> SaveAsync(UserClaim claim);
    Task<ClaimValue[]> GetUserClaimsAsync(Guid appId, Guid userId);
    Task DeleteByIdAsync(Guid userId, Guid claimId);
}