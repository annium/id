using System;
using System.Threading.Tasks;
using Annium.Extensions.Mapper;
using LinqToDB;

namespace Annium.Id.Db
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

        public Task DeleteByIdAsync(Guid companyId, Guid userId, Guid claimId)
        {
            return context.CompanyUserClaims.DeleteAsync(uc => uc.CompanyId == companyId && uc.UserId == userId && uc.ClaimId == claimId);
        }
    }
}