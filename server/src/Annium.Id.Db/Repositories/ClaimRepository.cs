using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Extensions.Mapper;
using LinqToDB;

namespace Annium.Id.Db
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
            entity.Id = Guid.NewGuid();

            using(var db = context.GetDataConnection())
            {
                await db.InsertAsync(entity);
            }

            return mapper.Map<Claim>(entity);
        }

        public async Task<Claim[]> GetAllAsync(Guid appId)
        {
            var claims = await context.Claims
                .Where(c => c.AppId == appId)
                .ToArrayAsync();

            return claims.Select(mapper.Map<Claim>).ToArray();
        }

        public async Task<Claim> GetByIdAsync(Guid id)
        {
            var claim = await context.Claims
                .FirstOrDefaultAsync(c => c.Id == id);

            return mapper.Map<Claim>(claim);
        }

        public async Task<Claim> FindByKeyAsync(Guid appId, string key)
        {
            var claim = await context.Claims
                .FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

            return mapper.Map<Claim>(claim);
        }

        public async Task<Claim> UpdateAsync(Claim claim)
        {
            var entity = mapper.Map<Entities.Claim>(claim);

            await context.Claims
                .UpdateAsync(
                    c => c.Id == entity.Id,
                    u => new Entities.Claim
                    {
                        Key = entity.Key,
                            Name = entity.Name,
                    }
                );

            return mapper.Map<Claim>(entity);
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.Claims.DeleteAsync(u => u.Id == id);
        }
    }
}