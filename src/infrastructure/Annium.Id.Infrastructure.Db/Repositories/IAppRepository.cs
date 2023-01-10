using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Repositories;

public interface IAppRepository
{
    Task<App> CreateAsync(App app);
    Task<App[]> FindAllAsync(string name);
    Task<App[]> FindMyAsync(Guid ownerId);
    Task<App> GetByIdAsync(Guid id);
    Task<App> UpdateAsync(App app);
    Task UpdateApiTokenAsync(Guid appId, Guid apiToken);
    Task DeleteByIdAsync(Guid id);
}