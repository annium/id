using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using LinqToDB;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class UserRoleRepository : IUserRoleRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public UserRoleRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<UserRole> SaveAsync(UserRole userRole)
        {
            var entity = mapper.Map<Entities.UserRole>(userRole);

            if ((await context.UserRoles.CountAsync(ur => ur.UserId == entity.UserId && ur.RoleId == entity.RoleId)) == 0)
                using(var db = context.GetDataConnection())
                {
                    await db.InsertAsync(entity);
                }

            return mapper.Map<UserRole>(entity);
        }

        public async Task<Role[]> GetUserRolesAsync(Guid appId, Guid userId)
        {
            var raw = await context.Roles
                .InnerJoin(context.UserRoles, (r, ur) => ur.RoleId == r.Id, (r, ur) => new { r, ur })
                .LeftJoin(context.RoleClaims, (e, rc) => rc.RoleId == e.r.Id, (e, rc) => new { r = e.r, ur = e.ur, rc })
                .LeftJoin(context.Claims, (e, c) => e.rc.ClaimId == c.Id, (e, c) => new { r = e.r, ur = e.ur, rc = e.rc, c })
                .Where(e => e.r.AppId == appId && e.ur.UserId == userId)
                .ToArrayAsync();

            var roles = raw
                .GroupBy(e => e.r)
                .Select(g =>
                {
                    var role = g.Key;
                    role.Claims = g
                        .Where(e => e.rc.ClaimId != Guid.Empty)
                        .Select(
                            e => new Entities.ClaimValue { Id = e.c.Id, Key = e.c.Key, Name = e.c.Name, Value = e.rc.Value }
                        )
                        .ToList();

                    return role;
                })
                .ToArray();

            return roles.Select(mapper.Map<Role>).ToArray();
        }

        public Task DeleteByIdAsync(Guid userId, Guid roleId)
        {
            return context.UserRoles.DeleteAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }
    }
}