using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class CompanyRoleRepository : RepositoryBase, ICompanyRoleRepository
{
    public CompanyRoleRepository(ServerConnection db) : base(db)
    {
    }

    public async Task CreateAsync(CompanyRole role)
    {
        await Db.CompanyRoles.InsertAsync(role);
    }

    public async Task<IReadOnlyCollection<CompanyRole>> GetAllAsync(Guid appId)
    {
        var entities = await Db.CompanyRoles
            .LoadWith(x => x.Claims).ThenLoad(x => x.Claim)
            .Where(x => x.AppId == appId)
            .ToArrayAsync();

        return entities;
    }

    public async Task<CompanyRole?> TryGetByIdAsync(Guid id)
    {
        var entity = await Db.CompanyRoles
            .FirstOrDefaultAsync(c => c.Id == id);

        return entity;
    }

    public async Task<CompanyRole> GetByIdAsync(Guid id)
    {
        var entity = await Db.CompanyRoles
            .FirstOrDefaultAsync(c => c.Id == id);

        if (entity is null)
            throw new InvalidOperationException($"Role {id} not found");

        return entity;
    }

    public async Task<CompanyRole?> TryFindByKeyAsync(Guid appId, string key)
    {
        var entity = await Db.CompanyRoles
            .FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

        return entity;
    }

    public async Task UpdateAsync(CompanyRole role)
    {
        await Db.CompanyRoles.UpdateAsync(role);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await Db.CompanyRoles.DeleteAsync(x => x.Id == id);
    }
}