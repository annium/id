using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Repositories
{
    public interface IAppRepository
    {
        Task<App> CreateAsync(App app);
        Task<App[]> FindAllAsync();
        Task<App[]> FindMyAsync(Guid ownerId);
        Task<App> GetByIdAsync(Guid id);
        Task<App> FindByApiTokenAsync(Guid token);
        Task<App> UpdateAsync(App app);
        Task UpdateApiTokenAsync(Guid appId, Guid apiToken);
        Task DeleteByIdAsync(Guid id);
    }
}