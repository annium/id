using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class AppRepository : RepositoryBase, IAppRepository
{
    public AppRepository(ServerConnection db) : base(db)
    {
    }

    public async Task CreateAsync(App app)
    {
        await Db.Apps.InsertAsync(app);
    }

    public async Task<IReadOnlyCollection<App>> FindAllAsync(string name)
    {
        IQueryable<App> query = Db.Apps;

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(x => x.Name.StartsWith(name));

        var entities = await query.ToArrayAsync();

        return entities;
    }

    public async Task<IReadOnlyCollection<App>> FindMyAsync(Guid ownerId)
    {
        var entities = await Db.Apps
            .Where(x => x.OwnerId == ownerId)
            .ToArrayAsync();

        return entities;
    }

    public async Task<App?> TryGetByIdAsync(Guid id)
    {
        var entity = await Db.Apps
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<App> GetByIdAsync(Guid id)
    {
        var entity = await Db.Apps
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            throw new InvalidOperationException($"App {id} not found");

        return entity;
    }

    public async Task UpdateAsync(App app)
    {
        await Db.Apps.UpdateAsync(app);
    }

    public async Task UpdateApiTokenAsync(Guid id, Guid apiToken)
    {
        var entity = await Db.Apps.SingleAsync(x => x.Id == id);
        entity.SetApiToken(apiToken);
        await Db.Apps.UpdateAsync(entity);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await Db.Apps.DeleteAsync(x => x.Id == id);
    }
}