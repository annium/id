using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class CompanyUserClaimRepository : ICompanyUserClaimRepository
    {
        private readonly IContext context;
        private readonly IMapper mapper;

        public CompanyUserClaimRepository(
            IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CompanyUserClaim> SaveAsync(CompanyUserClaim claim)
        {
            var entity = await context.CompanyUserClaims
                .FirstOrDefaultAsync(x => x.CompanyId == claim.CompanyId && x.UserId == claim.UserId && x.ClaimId == claim.ClaimId);

            if (entity is null)
            {
                entity = mapper.Map<Entities.CompanyUserClaim>(claim);
                context.CompanyUserClaims.Add(entity);
            }
            else
            {
                entity.Value = claim.Value;
            }

            await context.SaveChangesAsync();

            return mapper.Map<CompanyUserClaim>(entity);
        }

        public async Task<IReadOnlyDictionary<Guid, ClaimValue[]>> GetCompaniesUserClaimsAsync(Guid appId, Guid userId)
        {
            var raw = await context.CompanyUserClaims.AsNoTracking()
                .Include(x => x.Claim)
                .Where(x => x.Claim.AppId == appId && x.UserId == userId)
                .ToListAsync();

            return raw.GroupBy(x => x.CompanyId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(mapper.Map<ClaimValue>).ToArray()
                );
        }

        public async Task DeleteByIdAsync(Guid companyId, Guid userId, Guid claimId)
        {
            var entity = await context.CompanyUserClaims
                .FirstOrDefaultAsync(x => x.CompanyId == companyId && x.UserId == userId && x.ClaimId == claimId);

            if (entity is null)
                return;

            context.CompanyUserClaims.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}