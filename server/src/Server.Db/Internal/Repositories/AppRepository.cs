using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Db.Repositories;
using Server.Domain.Models;

namespace Server.Db.Internal.Repositories;

internal class AppRepository : IAppRepository
{
    private readonly ServerConnection _db;

    public AppRepository(ServerConnection db)
    {
        _db = db;
    }

    public async Task CreateAsync(App app)
    {
        await _db.Apps.InsertAsync(app);
    }

    public async Task<IReadOnlyCollection<App>> FindAllAsync(string name)
    {
        IQueryable<App> query = _db.Apps;

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(x => x.Name.StartsWith(name));

        var entities = await query.ToArrayAsync();

        return entities;
    }

    public async Task<IReadOnlyCollection<App>> FindMyAsync(Guid ownerId)
    {
        var entities = await _db.Apps
            .Where(x => x.OwnerId == ownerId)
            .ToArrayAsync();

        return entities;
    }

    public async Task<App?> TryGetByIdAsync(Guid id)
    {
        var entity = await _db.Apps
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<App> GetByIdAsync(Guid id)
    {
        var entity = await _db.Apps
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            throw new InvalidOperationException($"App {id} not found");

        return entity;
    }

    public async Task UpdateAsync(App app)
    {
        await _db.Apps.UpdateAsync(app);
    }

    public async Task UpdateApiTokenAsync(Guid id, Guid apiToken)
    {
        var entity = await _db.Apps.SingleAsync(x => x.Id == id);
        entity.SetApiToken(apiToken);
        await _db.Apps.UpdateAsync(entity);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await _db.Apps.DeleteAsync(x => x.Id == id);
    }
}