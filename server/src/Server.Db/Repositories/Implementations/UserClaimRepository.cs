using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class UserClaimRepository : RepositoryBase, IUserClaimRepository
{
    public UserClaimRepository(ServerConnection db) : base(db)
    {
    }

    public async Task SaveAsync(UserClaim claim)
    {
        await Db.UserClaims.InsertOrUpdateAsync(claim);
    }

    public async Task<IReadOnlyCollection<UserClaim>> GetUserClaimsAsync(Guid appId, Guid userId)
    {
        var entities = await Db.UserClaims
            .LoadWith(x => x.Claim)
            .Where(x => x.Claim.AppId == appId && x.UserId == userId)
            .ToArrayAsync();

        return entities;
    }

    public async Task DeleteByIdAsync(Guid userId, Guid claimId)
    {
        await Db.UserClaims.DeleteAsync(x => x.UserId == userId && x.ClaimId == claimId);
    }
}