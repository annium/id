using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> CreateAsync(Role role);
        Task<Role[]> GetAllAsync(Guid appId);
        Task<Role?> GetByIdAsync(Guid id);
        Task<Role?> FindByKeyAsync(Guid appId, string key);
        Task<Role> UpdateAsync(Role role);
        Task DeleteByIdAsync(Guid id);
    }
}