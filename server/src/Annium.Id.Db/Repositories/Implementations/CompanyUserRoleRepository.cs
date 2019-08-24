using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using LinqToDB;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class CompanyUserRoleRepository : ICompanyUserRoleRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public CompanyUserRoleRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CompanyUserRole> SaveAsync(CompanyUserRole userRole)
        {
            var entity = mapper.Map<Entities.CompanyUserRole>(userRole);

            if ((await context.CompanyUserRoles
                    .CountAsync(ur => ur.CompanyId == entity.CompanyId && ur.UserId == entity.UserId && ur.RoleId == entity.RoleId)) == 0)
                using(var db = context.GetDataConnection())
                {
                    await db.InsertAsync(entity);
                }

            return mapper.Map<CompanyUserRole>(entity);
        }

        public async Task<IReadOnlyDictionary<Guid, CompanyRole[]>> GetCompaniesUserRolesAsync(Guid appId, Guid userId)
        {
            var raw = await context.CompanyRoles
                .InnerJoin(context.CompanyUserRoles, (r, ur) => ur.RoleId == r.Id, (r, ur) => new { r, ur })
                .LeftJoin(context.CompanyRoleClaims, (e, rc) => rc.RoleId == e.r.Id, (e, rc) => new { r = e.r, ur = e.ur, rc })
                .LeftJoin(context.CompanyClaims, (e, c) => e.rc.ClaimId == c.Id, (e, c) => new { r = e.r, ur = e.ur, rc = e.rc, c })
                .Where(e => e.r.AppId == appId && e.ur.UserId == userId)
                .ToArrayAsync();

            var roles = raw
                .GroupBy(e => e.ur.CompanyId)
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(r => r.r)
                    .Select(r =>
                    {
                        var role = r.Key;
                        role.Claims = r
                            .Where(e => e.rc.ClaimId != Guid.Empty)
                            .Select(
                                e => new Entities.ClaimValue { Id = e.c.Id, Key = e.c.Key, Name = e.c.Name, Value = e.rc.Value }
                            )
                            .ToList();

                        return role;
                    })
                    .Select(mapper.Map<CompanyRole>)
                    .ToArray()
                );

            return roles;
        }

        public Task DeleteByIdAsync(Guid companyId, Guid userId, Guid roleId)
        {
            return context.CompanyUserRoles.DeleteAsync(ur => ur.CompanyId == companyId && ur.UserId == userId && ur.RoleId == roleId);
        }
    }
}