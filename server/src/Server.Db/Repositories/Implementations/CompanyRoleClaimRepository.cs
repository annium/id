using System;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class CompanyRoleClaimRepository : RepositoryBase, ICompanyRoleClaimRepository
{
    public CompanyRoleClaimRepository(ServerConnection db) : base(db)
    {
    }

    public async Task SaveAsync(CompanyRoleClaim claim)
    {
        await Db.CompanyRoleClaims.InsertOrUpdateAsync(claim);
    }

    public async Task DeleteByIdAsync(Guid roleId, Guid claimId)
    {
        await Db.CompanyRoleClaims.DeleteAsync(x => x.RoleId == roleId && x.ClaimId == claimId);
    }
}