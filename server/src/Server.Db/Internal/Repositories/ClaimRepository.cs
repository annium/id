using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Db.Repositories;
using Server.Domain.Models;

namespace Server.Db.Internal.Repositories;

internal class ClaimRepository : RepositoryBase, IClaimRepository
{
    public ClaimRepository(ServerConnection db) : base(db)
    {
    }

    public async Task CreateAsync(Claim claim)
    {
        await Db.Claims.InsertAsync(claim);
    }

    public async Task<IReadOnlyCollection<Claim>> GetAllAsync(Guid appId)
    {
        var entities = await Db.Claims
            .Where(x => x.AppId == appId)
            .ToArrayAsync();

        return entities;
    }

    public async Task<Claim?> TryGetByIdAsync(Guid id)
    {
        var entity = await Db.Claims
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<Claim> GetByIdAsync(Guid id)
    {
        var entity = await Db.Claims
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            throw new InvalidOperationException($"Claim {id} not found");

        return entity;
    }

    public async Task<Claim?> TryFindByKeyAsync(Guid appId, string key)
    {
        var entity = await Db.Claims
            .FirstOrDefaultAsync(x => x.AppId == appId && x.Key == key);

        return entity;
    }

    public async Task UpdateAsync(Claim claim)
    {
        await Db.Claims.UpdateAsync(claim);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await Db.Claims.DeleteAsync(x => x.Id == id);
    }
}