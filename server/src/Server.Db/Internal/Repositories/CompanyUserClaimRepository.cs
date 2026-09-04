using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions;
using LinqToDB;
using LinqToDB.Async;
using Server.Db.Repositories;
using Server.Domain.Models;

namespace Server.Db.Internal.Repositories;

internal class CompanyUserClaimRepository : ICompanyUserClaimRepository
{
    private readonly ServerConnection _db;

    public CompanyUserClaimRepository(ServerConnection db)
    {
        _db = db;
    }

    public async Task SaveAsync(CompanyUserClaim claim)
    {
        await _db.CompanyUserClaims.InsertOrUpdateAsync(claim);
    }

    public async Task<IReadOnlyDictionary<Guid, IReadOnlyCollection<CompanyUserClaim>>> GetCompaniesUserClaimsAsync(
        Guid appId,
        Guid userId
    )
    {
        var entities = await _db
            .CompanyUserClaims.LoadWith(x => x.Claim)
            .Where(x => x.Claim.AppId == appId && x.UserId == userId)
            .ToArrayAsync();

        return entities
            .GroupBy(x => x.CompanyId)
            .ToDictionary(x => x.Key, x => (IReadOnlyCollection<CompanyUserClaim>)x.ToArray());
    }

    public async Task DeleteByIdAsync(Guid companyId, Guid userId, Guid claimId)
    {
        await _db.CompanyUserClaims.DeleteAsync(x =>
            x.CompanyId == companyId && x.UserId == userId && x.ClaimId == claimId
        );
    }
}
