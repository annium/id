using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class ClaimRepository : IClaimRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public ClaimRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Claim> CreateAsync(Claim claim)
        {
            var entity = mapper.Map<Entities.Claim>(claim);

            context.Claims.Add(entity);
            await context.SaveChangesAsync();

            return mapper.Map<Claim>(entity);
        }

        public async Task<Claim[]> GetAllAsync(Guid appId)
        {
            var claims = await context.Claims.AsNoTracking()
                .Where(x => x.AppId == appId)
                .ToArrayAsync();

            return claims.Select(mapper.Map<Claim>).ToArray();
        }

        public async Task<Claim> GetByIdAsync(Guid id)
        {
            var claim = await context.Claims.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<Claim>(claim);
        }

        public async Task<Claim> FindByKeyAsync(Guid appId, string key)
        {
            var claim = await context.Claims.AsNoTracking()
                .FirstOrDefaultAsync(x => x.AppId == appId && x.Key == key);

            return mapper.Map<Claim>(claim);
        }

        public async Task<Claim> UpdateAsync(Claim claim)
        {
            var entity = await context.Claims
                .SingleAsync(x => x.Id == claim.Id);

            entity.Key = claim.Key;
            entity.Name = claim.Name;

            await context.SaveChangesAsync();

            return mapper.Map<Claim>(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.Claims
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return;

            context.Claims.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}