using System;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class RoleClaimRepository : RepositoryBase, IRoleClaimRepository
{
    public RoleClaimRepository(ServerConnection db) : base(db)
    {
    }

    public async Task SaveAsync(RoleClaim claim)
    {
        await Db.RoleClaims.InsertOrUpdateAsync(claim);
    }

    public async Task DeleteByIdAsync(Guid roleId, Guid claimId)
    {
        await Db.RoleClaims.DeleteAsync(x => x.RoleId == roleId && x.ClaimId == claimId);
    }
}