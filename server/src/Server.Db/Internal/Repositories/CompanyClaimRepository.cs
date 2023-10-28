using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions;
using LinqToDB;
using Server.Db.Repositories;
using Server.Domain.Models;

namespace Server.Db.Internal.Repositories;

internal class CompanyClaimRepository : ICompanyClaimRepository
{
    private readonly ServerConnection _db;

    public CompanyClaimRepository(ServerConnection db)
    {
        _db = db;
    }

    public async Task CreateAsync(CompanyClaim claim)
    {
        await _db.CompanyClaims.InsertAsync(claim);
    }

    public async Task<IReadOnlyCollection<CompanyClaim>> GetAllAsync(Guid appId)
    {
        var entities = await _db.CompanyClaims.Where(x => x.AppId == appId).ToArrayAsync();

        return entities;
    }

    public async Task<CompanyClaim?> TryGetByIdAsync(Guid id)
    {
        var entity = await _db.CompanyClaims.FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<CompanyClaim> GetByIdAsync(Guid id)
    {
        var entity = await _db.CompanyClaims.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            throw new InvalidOperationException($"Claim {id} not found");

        return entity;
    }

    public async Task<CompanyClaim?> TryFindByKeyAsync(Guid appId, string key)
    {
        var entity = await _db.CompanyClaims.FirstOrDefaultAsync(x => x.AppId == appId && x.Key == key);

        return entity;
    }

    public async Task UpdateAsync(CompanyClaim claim)
    {
        await _db.CompanyClaims.UpdateAsync(claim);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await _db.CompanyClaims.DeleteAsync(x => x.Id == id);
    }
}
