using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Extensions.Mapper;
using LinqToDB;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class CompanyRoleRepository : ICompanyRoleRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public CompanyRoleRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CompanyRole> CreateAsync(CompanyRole role)
        {
            var entity = mapper.Map<Entities.CompanyRole>(role);
            entity.Id = Guid.NewGuid();

            using(var db = context.GetDataConnection())
            {
                await db.InsertAsync(entity);
            }

            return mapper.Map<CompanyRole>(entity);
        }

        public async Task<CompanyRole[]> GetAllAsync(Guid appId)
        {
            var raw = await context.CompanyRoles
                .LeftJoin(context.CompanyRoleClaims, (r, rc) => rc.RoleId == r.Id, (r, rc) => new { r, rc })
                .LeftJoin(context.CompanyClaims, (rc, c) => rc.rc.ClaimId == c.Id, (rc, c) => new { r = rc.r, rc = rc.rc, c })
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

            return roles.Select(mapper.Map<CompanyRole>).ToArray();
        }

        public async Task<CompanyRole> GetByIdAsync(Guid id)
        {
            var role = await context.CompanyRoles
                .FirstOrDefaultAsync(c => c.Id == id);

            return mapper.Map<CompanyRole>(role);
        }

        public async Task<CompanyRole> FindByKeyAsync(Guid appId, string key)
        {
            var role = await context.CompanyRoles
                .FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

            return mapper.Map<CompanyRole>(role);
        }

        public async Task<CompanyRole> UpdateAsync(CompanyRole role)
        {
            var entity = mapper.Map<Entities.CompanyRole>(role);

            await context.CompanyRoles
                .UpdateAsync(
                    c => c.Id == entity.Id,
                    u => new Entities.CompanyRole
                    {
                        Key = entity.Key,
                            Name = entity.Name,
                    }
                );

            entity.Claims = await context.CompanyRoleClaims
                .Where(rc => rc.RoleId == entity.Id)
                .InnerJoin(context.CompanyClaims, (rc, c) => rc.ClaimId == c.Id, (rc, c) => new { rc, c })
                .Select(
                    rc => new Entities.ClaimValue { Id = rc.c.Id, Key = rc.c.Key, Name = rc.c.Name, Value = rc.rc.Value }
                )
                .ToListAsync();

            return mapper.Map<CompanyRole>(entity);
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.CompanyRoles.DeleteAsync(u => u.Id == id);
        }
    }
}