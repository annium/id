using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqToDB;

namespace Annium.Id.Db
{
    internal class CompanyClaimRepository : ICompanyClaimRepository
    {
        private readonly Entities.IContext context;

        private readonly IMapper mapper;

        public CompanyClaimRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CompanyClaim> CreateAsync(CompanyClaim claim)
        {
            var entity = mapper.Map<Entities.CompanyClaim>(claim);
            entity.Id = Guid.NewGuid();

            using(var db = context.GetDataConnection())
            {
                await db.InsertAsync(entity);
            }

            return mapper.Map<CompanyClaim>(entity);
        }

        public async Task<CompanyClaim[]> GetAllAsync(Guid appId)
        {
            var claims = await context.CompanyClaims
                .Where(c => c.AppId == appId)
                .ToArrayAsync();

            return claims.Select(mapper.Map<CompanyClaim>).ToArray();
        }

        public async Task<CompanyClaim> GetByIdAsync(Guid id)
        {
            var claim = await context.CompanyClaims
                .FirstOrDefaultAsync(c => c.Id == id);

            return mapper.Map<CompanyClaim>(claim);
        }

        public async Task<CompanyClaim> FindByKeyAsync(Guid appId, string key)
        {
            var claim = await context.CompanyClaims
                .FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

            return mapper.Map<CompanyClaim>(claim);
        }

        public async Task<CompanyClaim> UpdateAsync(CompanyClaim claim)
        {
            var entity = mapper.Map<Entities.CompanyClaim>(claim);

            await context.CompanyClaims
                .UpdateAsync(
                    c => c.Id == entity.Id,
                    u => new Entities.CompanyClaim
                    {
                        Key = entity.Key,
                            Name = entity.Name,
                    }
                );

            return mapper.Map<CompanyClaim>(entity);
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.CompanyClaims.DeleteAsync(u => u.Id == id);
        }
    }
}