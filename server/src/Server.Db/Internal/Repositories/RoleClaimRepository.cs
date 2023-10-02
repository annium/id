using System;
using System.Threading.Tasks;
using Annium.linq2db.Extensions;
using LinqToDB;
using Server.Db.Repositories;
using Server.Domain.Models;

namespace Server.Db.Internal.Repositories;

internal class RoleClaimRepository : IRoleClaimRepository
{
    private readonly ServerConnection _db;

    public RoleClaimRepository(ServerConnection db)
    {
        _db = db;
    }

    public async Task SaveAsync(RoleClaim claim)
    {
        await _db.RoleClaims.InsertOrUpdateAsync(claim);
    }

    public async Task DeleteByIdAsync(Guid roleId, Guid claimId)
    {
        await _db.RoleClaims.DeleteAsync(x => x.RoleId == roleId && x.ClaimId == claimId);
    }
}