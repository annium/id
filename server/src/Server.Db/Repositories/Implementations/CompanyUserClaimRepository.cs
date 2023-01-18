using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class CompanyUserClaimRepository : RepositoryBase, ICompanyUserClaimRepository
{
    public CompanyUserClaimRepository(ServerConnection db) : base(db)
    {
    }

    public async Task SaveAsync(CompanyUserClaim claim)
    {
        await Db.CompanyUserClaims.InsertOrUpdateAsync(claim);
    }

    public async Task<IReadOnlyDictionary<Guid, IReadOnlyCollection<CompanyUserClaim>>> GetCompaniesUserClaimsAsync(Guid appId, Guid userId)
    {
        var entities = await Db.CompanyUserClaims
            .LoadWith(x => x.Claim)
            .Where(x => x.Claim.AppId == appId && x.UserId == userId)
            .ToArrayAsync();

        return entities.GroupBy(x => x.CompanyId)
            .ToDictionary(
                x => x.Key,
                x => (IReadOnlyCollection<CompanyUserClaim>) x.ToArray()
            );
    }

    public async Task DeleteByIdAsync(Guid companyId, Guid userId, Guid claimId)
    {
        await Db.CompanyUserClaims.DeleteAsync(x => x.CompanyId == companyId && x.UserId == userId && x.ClaimId == claimId);
    }
}