using System;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Server.Db.Repositories;

public interface IAppRepository
{
    Task<App> CreateAsync(App app);
    Task<App[]> FindAllAsync(string name);
    Task<App[]> FindMyAsync(Guid ownerId);
    Task<App?> TryGetByIdAsync(Guid id);
    Task<App> GetByIdAsync(Guid id);
    Task<App> UpdateAsync(App app);
    Task UpdateApiTokenAsync(Guid appId, Guid apiToken);
    Task DeleteByIdAsync(Guid id);
}