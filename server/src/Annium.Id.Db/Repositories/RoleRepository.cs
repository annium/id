using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Extensions.Mapper;
using LinqToDB;

namespace Annium.Id.Db
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public RoleRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Role> CreateAsync(Role role)
        {
            var entity = mapper.Map<Entities.Role>(role);
            entity.Id = Guid.NewGuid();

            using(var db = context.GetDataConnection())
            {
                await db.InsertAsync(entity);
            }

            return mapper.Map<Role>(entity);
        }

        public async Task<Role[]> GetAllAsync(Guid appId)
        {
            var raw = await context.Roles
                .LeftJoin(context.RoleClaims, (r, rc) => rc.RoleId == r.Id, (r, rc) => new { r, rc })
                .LeftJoin(context.Claims, (rc, c) => rc.rc.ClaimId == c.Id, (rc, c) => new { r = rc.r, rc = rc.rc, c })
                .Where(rc => rc.r.AppId == appId)
                .ToArrayAsync();

            var roles = raw
                .GroupBy(rc => rc.r)
                .Select(g =>
                {
                    var role = g.Key;
                    role.Claims = g.Where(rc => rc.rc.ClaimId != Guid.Empty).Select(
                        rc => new Entities.ClaimValue { Id = rc.c.Id, Key = rc.c.Key, Name = rc.c.Name, Value = rc.rc.Value }
                    ).ToList();

                    return role;
                })
                .ToArray();

            return roles.Select(mapper.Map<Role>).ToArray();
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            var role = await context.Roles
                .FirstOrDefaultAsync(c => c.Id == id);

            return mapper.Map<Role>(role);
        }

        public async Task<Role> FindByKeyAsync(Guid appId, string key)
        {
            var role = await context.Roles
                .FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

            return mapper.Map<Role>(role);
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            var entity = mapper.Map<Entities.Role>(role);

            await context.Roles
                .UpdateAsync(
                    c => c.Id == entity.Id,
                    u => new Entities.Role
                    {
                        Key = entity.Key,
                            Name = entity.Name,
                    }
                );

            entity.Claims = await context.RoleClaims
                .Where(rc => rc.RoleId == entity.Id)
                .InnerJoin(context.Claims, (rc, c) => rc.ClaimId == c.Id, (rc, c) => new { rc, c })
                .Select(
                    rc => new Entities.ClaimValue { Id = rc.c.Id, Key = rc.c.Key, Name = rc.c.Name, Value = rc.rc.Value }
                )
                .ToListAsync();

            return mapper.Map<Role>(entity);
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.Roles.DeleteAsync(u => u.Id == id);
        }
    }
}