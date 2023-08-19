using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Db.Repositories;
using Server.Domain.Models;

namespace Server.Db.Internal.Repositories;

internal class UserRoleRepository : RepositoryBase, IUserRoleRepository
{
    public UserRoleRepository(ServerConnection db) : base(db)
    {
    }

    public async Task SaveAsync(UserRole userRole)
    {
        await Db.UserRoles.InsertOrUpdateAsync(userRole);
    }

    public async Task<IReadOnlyCollection<Role>> GetUserRolesAsync(Guid appId, Guid userId)
    {
        var entities = await Db.UserRoles
            .LoadWith(x => x.Role).ThenLoad(x => x.Claims).ThenLoad(x => x.Claim)
            .Where(x => x.Role.AppId == appId && x.UserId == userId)
            .ToArrayAsync();

        return entities.Select(x => x.Role).ToArray();
    }

    public async Task DeleteByIdAsync(Guid userId, Guid roleId)
    {
        await Db.UserRoles.DeleteAsync(x => x.UserId == userId && x.RoleId == roleId);
    }
}