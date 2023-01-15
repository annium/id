using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface IUserClaimRepository
{
    Task SaveAsync(UserClaim claim);
    Task<IReadOnlyCollection<UserClaim>> GetUserClaimsAsync(Guid appId, Guid userId);
    Task DeleteByIdAsync(Guid userId, Guid claimId);
}