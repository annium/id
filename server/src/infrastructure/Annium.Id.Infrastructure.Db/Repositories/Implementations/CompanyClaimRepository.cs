using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Infrastructure.Db.Repositories.Implementations
{
    internal class CompanyClaimRepository : ICompanyClaimRepository
    {
        private readonly IContext context;
        private readonly IMapper mapper;

        public CompanyClaimRepository(
            IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CompanyClaim> CreateAsync(CompanyClaim claim)
        {
            var entity = mapper.Map<Entities.CompanyClaim>(claim);

            context.CompanyClaims.Add(entity);
            await context.SaveChangesAsync();

            return mapper.Map<CompanyClaim>(entity);
        }

        public async Task<CompanyClaim[]> GetAllAsync(Guid appId)
        {
            var claims = await context.CompanyClaims.AsNoTracking()
                .Where(x => x.AppId == appId)
                .ToArrayAsync();

            return claims.Select(mapper.Map<CompanyClaim>).ToArray();
        }

        public async Task<CompanyClaim> GetByIdAsync(Guid id)
        {
            var claim = await context.CompanyClaims.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<CompanyClaim>(claim);
        }

        public async Task<CompanyClaim> FindByKeyAsync(Guid appId, string key)
        {
            var claim = await context.CompanyClaims.AsNoTracking()
                .FirstOrDefaultAsync(x => x.AppId == appId && x.Key == key);

            return mapper.Map<CompanyClaim>(claim);
        }

        public async Task<CompanyClaim> UpdateAsync(CompanyClaim claim)
        {
            var entity = await context.CompanyClaims
                .SingleAsync(x => x.Id == claim.Id);

            entity.Key = claim.Key;
            entity.Name = claim.Name;

            await context.SaveChangesAsync();

            return mapper.Map<CompanyClaim>(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.CompanyClaims
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return;

            context.CompanyClaims.Remove(entity);
            ;

            await context.SaveChangesAsync();
        }
    }
}