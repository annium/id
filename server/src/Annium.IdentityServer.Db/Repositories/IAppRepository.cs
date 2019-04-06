using System;
using System.Threading.Tasks;

namespace Annium.IdentityServer.Db
{
    public interface IAppRepository
    {
        Task<App> CreateAsync(App app);

        Task<App[]> GetAllAsync();

        Task<App> GetByIdAsync(Guid id);

        Task<App> FindByNameAsync(string name);

        Task<App> FindByApiTokenAsync(Guid token);

        Task UpdateAsync(App app);

        Task UpdateApiTokenAsync(Guid appId, Guid apiToken);

        Task DeleteByIdAsync(Guid id);
    }
}