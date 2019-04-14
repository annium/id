using System;
using System.Threading.Tasks;

namespace Annium.Id.Db
{
    public interface IUserRoleRepository
    {
        Task<UserRole> SaveAsync(UserRole userRole);

        Task DeleteByIdAsync(Guid userId, Guid roleId);
    }
}