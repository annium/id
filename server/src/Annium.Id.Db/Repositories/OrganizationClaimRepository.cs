using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqToDB;

namespace Annium.Id.Db
{
    internal class OrganizationClaimRepository : IOrganizationClaimRepository
    {
        private readonly Entities.IContext context;

        private readonly IMapper mapper;

        public OrganizationClaimRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<OrganizationClaim> CreateAsync(OrganizationClaim claim)
        {
            var entity = mapper.Map<Entities.OrganizationClaim>(claim);
            entity.Id = Guid.NewGuid();

            using(var db = context.GetDataConnection())
            {
                await db.InsertAsync(entity);
            }

            return mapper.Map<OrganizationClaim>(entity);
        }

        public async Task<OrganizationClaim[]> GetAllAsync(Guid appId)
        {
            var claims = await context.OrganizationClaims
                .Where(c => c.AppId == appId)
                .ToArrayAsync();

            return claims.Select(mapper.Map<OrganizationClaim>).ToArray();
        }

        public async Task<OrganizationClaim> GetByIdAsync(Guid id)
        {
            var claim = await context.OrganizationClaims
                .FirstOrDefaultAsync(c => c.Id == id);

            return mapper.Map<OrganizationClaim>(claim);
        }

        public async Task<OrganizationClaim> FindByKeyAsync(Guid appId, string key)
        {
            var claim = await context.OrganizationClaims
                .FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

            return mapper.Map<OrganizationClaim>(claim);
        }

        public async Task<OrganizationClaim> UpdateAsync(OrganizationClaim claim)
        {
            var entity = mapper.Map<Entities.OrganizationClaim>(claim);

            await context.OrganizationClaims
                .UpdateAsync(
                    c => c.Id == entity.Id,
                    u => new Entities.OrganizationClaim
                    {
                        Key = entity.Key,
                            Name = entity.Name,
                    }
                );

            return mapper.Map<OrganizationClaim>(entity);
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.OrganizationClaims.DeleteAsync(u => u.Id == id);
        }
    }
}