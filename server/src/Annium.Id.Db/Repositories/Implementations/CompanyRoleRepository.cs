using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

            context.CompanyRoles.Add(entity);
            await context.SaveChangesAsync();

            return mapper.Map<CompanyRole>(entity);
        }

        public async Task<CompanyRole[]> GetAllAsync(Guid appId)
        {
            var raw = await context.CompanyRoles.AsNoTracking()
                .Include(x => x.Claims).ThenInclude(x => x.Claim)
                .ToListAsync();

            return raw.Select(mapper.Map<CompanyRole>).ToArray();
        }

        public async Task<CompanyRole> GetByIdAsync(Guid id)
        {
            var role = await context.CompanyRoles.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return mapper.Map<CompanyRole>(role);
        }

        public async Task<CompanyRole> FindByKeyAsync(Guid appId, string key)
        {
            var role = await context.CompanyRoles.AsNoTracking()
                .FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

            return mapper.Map<CompanyRole>(role);
        }

        public async Task<CompanyRole> UpdateAsync(CompanyRole role)
        {
            var entity = await context.CompanyRoles
                .Include(x => x.Claims).ThenInclude(x => x.Claim)
                .SingleAsync(x => x.Id == role.Id);

            entity.Key = role.Key;
            entity.Name = role.Name;

            await context.SaveChangesAsync();

            return mapper.Map<CompanyRole>(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.CompanyRoles
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return;

            context.CompanyRoles.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}