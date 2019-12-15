using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly IContext context;
        private readonly IMapper mapper;

        public RoleRepository(
            IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Role> CreateAsync(Role role)
        {
            var entity = mapper.Map<Entities.Role>(role);

            context.Roles.Add(entity);
            await context.SaveChangesAsync();

            return mapper.Map<Role>(entity);
        }

        public async Task<Role[]> GetAllAsync(Guid appId)
        {
            var raw = await context.Roles.AsNoTracking()
                .Include(x => x.Claims).ThenInclude(x => x.Claim)
                .ToListAsync();

            return raw.Select(mapper.Map<Role>).ToArray();
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            var role = await context.Roles.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return mapper.Map<Role>(role);
        }

        public async Task<Role> FindByKeyAsync(Guid appId, string key)
        {
            var role = await context.Roles.AsNoTracking()
                .FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

            return mapper.Map<Role>(role);
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            var entity = await context.Roles
                .Include(x => x.Claims).ThenInclude(x => x.Claim)
                .SingleAsync(x => x.Id == role.Id);

            entity.Key = role.Key;
            entity.Name = role.Name;

            await context.SaveChangesAsync();

            return mapper.Map<Role>(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.Roles
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return;

            context.Roles.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}