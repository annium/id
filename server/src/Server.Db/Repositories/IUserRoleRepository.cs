using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface IUserRoleRepository
{
    Task SaveAsync(UserRole userRole);
    Task<IReadOnlyCollection<Role>> GetUserRolesAsync(Guid appId, Guid userId);
    Task DeleteByIdAsync(Guid userId, Guid roleId);
}
