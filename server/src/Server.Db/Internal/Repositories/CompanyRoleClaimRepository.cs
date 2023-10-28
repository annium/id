using System;
using System.Threading.Tasks;
using Annium.linq2db.Extensions;
using LinqToDB;
using Server.Db.Repositories;
using Server.Domain.Models;

namespace Server.Db.Internal.Repositories;

internal class CompanyRoleClaimRepository : ICompanyRoleClaimRepository
{
    private readonly ServerConnection _db;

    public CompanyRoleClaimRepository(ServerConnection db)
    {
        _db = db;
    }

    public async Task SaveAsync(CompanyRoleClaim claim)
    {
        await _db.CompanyRoleClaims.InsertOrUpdateAsync(claim);
    }

    public async Task DeleteByIdAsync(Guid roleId, Guid claimId)
    {
        await _db.CompanyRoleClaims.DeleteAsync(x => x.RoleId == roleId && x.ClaimId == claimId);
    }
}
