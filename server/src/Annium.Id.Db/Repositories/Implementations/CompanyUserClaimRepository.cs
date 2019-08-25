using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using LinqToDB;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class CompanyUserClaimRepository : ICompanyUserClaimRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public CompanyUserClaimRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CompanyUserClaim> SaveAsync(CompanyUserClaim claim)
        {
            var entity = mapper.Map<Entities.CompanyUserClaim>(claim);

            using(var db = context.GetDataConnection())
            {
                await db.InsertOrReplaceAsync(entity);
            }

            return mapper.Map<CompanyUserClaim>(entity);
        }

        public async Task<IReadOnlyDictionary<Guid, ClaimValue[]>> GetCompaniesUserClaimsAsync(Guid appId, Guid userId)
        {
            var raw = await context.CompanyClaims
                .InnerJoin(context.CompanyUserClaims, (c, uc) => uc.ClaimId == c.Id, (c, uc) => new { c, uc })
                .Where(e => e.c.AppId == appId && e.uc.UserId == userId)
                .ToArrayAsync();

            var claims = raw
                .GroupBy(e => e.uc.CompanyId)
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(e => e.c)
                    .Select(e =>
                    {
                        var c = e.Key;

                        return new Entities.ClaimValue { Id = c.Id, Key = c.Key, Name = c.Name, Value = g.First().uc.Value };
                    })
                    .Select(mapper.Map<ClaimValue>)
                    .ToArray()
                );

            return claims;
        }

        public Task DeleteByIdAsync(Guid companyId, Guid userId, Guid claimId)
        {
            return context.CompanyUserClaims.DeleteAsync(uc => uc.CompanyId == companyId && uc.UserId == userId && uc.ClaimId == claimId);
        }
    }
}