using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface IAppRepository
{
    Task CreateAsync(App app);
    Task<IReadOnlyCollection<App>> FindAllAsync(string name);
    Task<IReadOnlyCollection<App>> FindMyAsync(Guid ownerId);
    Task<App?> TryGetByIdAsync(Guid id);
    Task<App> GetByIdAsync(Guid id);
    Task UpdateAsync(App app);
    Task UpdateApiTokenAsync(Guid id, Guid apiToken);
    Task DeleteByIdAsync(Guid id);
}
