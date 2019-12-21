using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Db.Repositories
{
    public interface IAppRepository
    {
        Task<App> CreateAsync(App app);
        Task<App[]> GetAllAsync();
        Task<App> GetByIdAsync(Guid id);
        Task<App> FindByApiTokenAsync(Guid token);
        Task<App> UpdateAsync(App app);
        Task UpdateApiTokenAsync(Guid appId, Guid apiToken);
        Task DeleteByIdAsync(Guid id);
    }
}