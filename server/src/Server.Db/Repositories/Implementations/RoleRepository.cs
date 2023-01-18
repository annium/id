using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class RoleRepository : RepositoryBase, IRoleRepository
{
    public RoleRepository(ServerConnection db) : base(db)
    {
    }

    public async Task CreateAsync(Role role)
    {
        await Db.Roles.InsertAsync(role);
    }

    public async Task<IReadOnlyCollection<Role>> GetAllAsync(Guid appId)
    {
        var entities = await Db.Roles
            .LoadWith(x => x.Claims).ThenLoad(x => x.Claim)
            .ToArrayAsync();

        return entities;
    }

    public async Task<Role?> TryGetByIdAsync(Guid id)
    {
        var entity = await Db.Roles
            .FirstOrDefaultAsync(c => c.Id == id);

        return entity;
    }

    public async Task<Role> GetByIdAsync(Guid id)
    {
        var entity = await Db.Roles
            .FirstOrDefaultAsync(c => c.Id == id);

        if (entity is null)
            throw new InvalidOperationException($"Role {id} not found");

        return entity;
    }

    public async Task<Role?> TryFindByKeyAsync(Guid appId, string key)
    {
        var entity = await Db.Roles
            .FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

        return entity;
    }

    public async Task UpdateAsync(Role role)
    {
        await Db.Roles.UpdateAsync(role);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await Db.Roles.DeleteAsync(x => x.Id == id);
    }
}