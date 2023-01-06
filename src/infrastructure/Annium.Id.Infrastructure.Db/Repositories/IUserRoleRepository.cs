using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Repositories;

public interface IUserRoleRepository
{
    Task<UserRole> SaveAsync(UserRole userRole);
    Task<Role[]> GetUserRolesAsync(Guid appId, Guid userId);
    Task DeleteByIdAsync(Guid userId, Guid roleId);
}