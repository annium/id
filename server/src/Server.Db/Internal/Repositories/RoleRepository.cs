using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions;
using LinqToDB;
using Server.Db.Repositories;
using Server.Domain.Models;

namespace Server.Db.Internal.Repositories;

internal class RoleRepository : IRoleRepository
{
    private readonly ServerConnection _db;

    public RoleRepository(ServerConnection db)
    {
        _db = db;
    }

    public async Task CreateAsync(Role role)
    {
        await _db.Roles.InsertAsync(role);
    }

    public async Task<IReadOnlyCollection<Role>> GetAllAsync(Guid appId)
    {
        var entities = await _db.Roles
            .LoadWith(x => x.Claims)
            .ThenLoad(x => x.Claim)
            .Where(x => x.AppId == appId)
            .ToArrayAsync();

        return entities;
    }

    public async Task<Role?> TryGetByIdAsync(Guid id)
    {
        var entity = await _db.Roles.FirstOrDefaultAsync(c => c.Id == id);

        return entity;
    }

    public async Task<Role> GetByIdAsync(Guid id)
    {
        var entity = await _db.Roles.FirstOrDefaultAsync(c => c.Id == id);

        if (entity is null)
            throw new InvalidOperationException($"Role {id} not found");

        return entity;
    }

    public async Task<Role?> TryFindByKeyAsync(Guid appId, string key)
    {
        var entity = await _db.Roles.FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

        return entity;
    }

    public async Task UpdateAsync(Role role)
    {
        await _db.Roles.UpdateAsync(role);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await _db.Roles.DeleteAsync(x => x.Id == id);
    }
}
