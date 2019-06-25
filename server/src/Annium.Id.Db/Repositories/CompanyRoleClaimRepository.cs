using System;
using System.Threading.Tasks;
using Annium.Extensions.Mapper;
using LinqToDB;

namespace Annium.Id.Db
{
    internal class CompanyRoleClaimRepository : ICompanyRoleClaimRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public CompanyRoleClaimRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CompanyRoleClaim> SaveAsync(CompanyRoleClaim claim)
        {
            var entity = mapper.Map<Entities.CompanyRoleClaim>(claim);

            using(var db = context.GetDataConnection())
            {
                await db.InsertOrReplaceAsync(entity);
            }

            return mapper.Map<CompanyRoleClaim>(entity);
        }

        public Task DeleteByIdAsync(Guid roleId, Guid claimId)
        {
            return context.CompanyRoleClaims.DeleteAsync(rc => rc.RoleId == roleId && rc.ClaimId == claimId);
        }
    }
}