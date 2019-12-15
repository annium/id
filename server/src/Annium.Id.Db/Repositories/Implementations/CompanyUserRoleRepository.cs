using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class CompanyUserRoleRepository : ICompanyUserRoleRepository
    {
        private readonly IContext context;
        private readonly IMapper mapper;

        public CompanyUserRoleRepository(
            IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CompanyUserRole> SaveAsync(CompanyUserRole userRole)
        {
            var entity = await context.CompanyUserRoles
                .FirstOrDefaultAsync(x => x.CompanyId == userRole.CompanyId && x.UserId == userRole.UserId && x.RoleId == userRole.RoleId);

            if (entity is null)
            {
                entity = mapper.Map<Entities.CompanyUserRole>(userRole);
                context.CompanyUserRoles.Add(entity);
                await context.SaveChangesAsync();
            }

            return mapper.Map<CompanyUserRole>(entity);
        }

        public async Task<IReadOnlyDictionary<Guid, CompanyRole[]>> GetCompaniesUserRolesAsync(Guid appId, Guid userId)
        {
            var raw = await context.CompanyUserRoles.AsNoTracking()
                .Include(x => x.Role).ThenInclude(x => x.Claims).ThenInclude(x => x.Claim)
                .Where(x => x.Role.AppId == appId && x.UserId == userId)
                .ToListAsync();

            return raw.GroupBy(x => x.CompanyId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(mapper.Map<CompanyRole>).ToArray()
                );
        }

        public async Task DeleteByIdAsync(Guid companyId, Guid userId, Guid roleId)
        {
            var entity = await context.CompanyUserRoles
                .FirstOrDefaultAsync(x => x.CompanyId == companyId && x.UserId == userId && x.RoleId == roleId);

            if (entity is null)
                return;

            context.CompanyUserRoles.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}