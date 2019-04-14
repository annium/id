using System;
using System.Threading.Tasks;
using AutoMapper;
using LinqToDB;

namespace Annium.Id.Db
{
    internal class OrganizationRoleClaimRepository : IOrganizationRoleClaimRepository
    {
        private readonly Entities.IContext context;

        private readonly IMapper mapper;

        public OrganizationRoleClaimRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<OrganizationRoleClaim> SaveAsync(OrganizationRoleClaim claim)
        {
            var entity = mapper.Map<Entities.OrganizationRoleClaim>(claim);

            using(var db = context.GetDataConnection())
            {
                await db.InsertOrReplaceAsync(entity);
            }

            return mapper.Map<OrganizationRoleClaim>(entity);
        }

        public Task DeleteByIdAsync(Guid roleId, Guid claimId)
        {
            return context.OrganizationRoleClaims.DeleteAsync(rc => rc.RoleId == roleId && rc.ClaimId == claimId);
        }
    }
}