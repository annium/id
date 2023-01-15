using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface IRoleRepository
{
    Task CreateAsync(Role role);
    Task<IReadOnlyCollection<Role>> GetAllAsync(Guid appId);
    Task<Role?> TryGetByIdAsync(Guid id);
    Task<Role> GetByIdAsync(Guid id);
    Task<Role?> TryFindByKeyAsync(Guid appId, string key);
    Task UpdateAsync(Role role);
    Task DeleteByIdAsync(Guid id);
}