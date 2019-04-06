using System;
using System.Threading.Tasks;

namespace Annium.IdentityServer.Db
{
    public interface IAppRepository
    {
        Task<App> CreateAsync(App app);

        Task<App> GetById(Guid id);

        Task<App> FindByNameAsync(string name);

        Task<App> FindByApiTokenAsync(Guid token);

        Task UpdateAsync(App app);

        Task UpdateApiTokenAsync(Guid appId, Guid apiToken);

        Task DeleteByIdAsync(Guid id);
    }
}