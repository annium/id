using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class UserClaimRepository : IUserClaimRepository
{
    private readonly ServerConnection _db;

    public UserClaimRepository(ServerConnection db)
    {
        _db = db;
    }

    public async Task SaveAsync(UserClaim claim)
    {
        await _db.UserClaims.InsertOrUpdateAsync(claim);
    }

    public async Task<IReadOnlyCollection<UserClaim>> GetUserClaimsAsync(Guid appId, Guid userId)
    {
        var entities = await _db.UserClaims
            .LoadWith(x => x.Claim)
            .Where(x => x.Claim.AppId == appId && x.UserId == userId)
            .ToArrayAsync();

        return entities;
    }

    public async Task DeleteByIdAsync(Guid userId, Guid claimId)
    {
        await _db.UserClaims.DeleteAsync(x => x.UserId == userId && x.ClaimId == claimId);
    }
}