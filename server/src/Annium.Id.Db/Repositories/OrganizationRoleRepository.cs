using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqToDB;

namespace Annium.Id.Db
{
    internal class OrganizationRoleRepository : IOrganizationRoleRepository
    {
        private readonly Entities.IContext context;

        private readonly IMapper mapper;

        public OrganizationRoleRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<OrganizationRole> CreateAsync(OrganizationRole role)
        {
            var entity = mapper.Map<Entities.OrganizationRole>(role);
            entity.Id = Guid.NewGuid();

            using(var db = context.GetDataConnection())
            {
                await db.InsertAsync(entity);
            }

            return mapper.Map<OrganizationRole>(entity);
        }

        public async Task<OrganizationRole[]> GetAllAsync(Guid appId)
        {
            var raw = await context.OrganizationRoles
                .LeftJoin(context.OrganizationRoleClaims, (r, rc) => rc.RoleId == r.Id, (r, rc) => new { r, rc })
                .LeftJoin(context.OrganizationClaims, (rc, c) => rc.rc.ClaimId == c.Id, (rc, c) => new { r = rc.r, rc = rc.rc, c })
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

            return roles.Select(mapper.Map<OrganizationRole>).ToArray();
        }

        public async Task<OrganizationRole> GetByIdAsync(Guid id)
        {
            var role = await context.OrganizationRoles
                .FirstOrDefaultAsync(c => c.Id == id);

            return mapper.Map<OrganizationRole>(role);
        }

        public async Task<OrganizationRole> FindByKeyAsync(Guid appId, string key)
        {
            var role = await context.OrganizationRoles
                .FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

            return mapper.Map<OrganizationRole>(role);
        }

        public async Task<OrganizationRole> UpdateAsync(OrganizationRole role)
        {
            var entity = mapper.Map<Entities.OrganizationRole>(role);

            await context.OrganizationRoles
                .UpdateAsync(
                    c => c.Id == entity.Id,
                    u => new Entities.OrganizationRole
                    {
                        Key = entity.Key,
                            Name = entity.Name,
                    }
                );

            entity.Claims = await context.OrganizationRoleClaims
                .Where(rc => rc.RoleId == entity.Id)
                .InnerJoin(context.OrganizationClaims, (rc, c) => rc.ClaimId == c.Id, (rc, c) => new { rc, c })
                .Select(
                    rc => new Entities.ClaimValue { Id = rc.c.Id, Key = rc.c.Key, Name = rc.c.Name, Value = rc.rc.Value }
                )
                .ToListAsync();

            return mapper.Map<OrganizationRole>(entity);
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.OrganizationRoles.DeleteAsync(u => u.Id == id);
        }
    }
}