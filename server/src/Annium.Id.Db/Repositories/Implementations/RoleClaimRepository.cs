using System;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using LinqToDB;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class RoleClaimRepository : IRoleClaimRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public RoleClaimRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<RoleClaim> SaveAsync(RoleClaim claim)
        {
            var entity = mapper.Map<Entities.RoleClaim>(claim);

            using(var db = context.GetDataConnection())
            {
                await db.InsertOrReplaceAsync(entity);
            }

            return mapper.Map<RoleClaim>(entity);
        }

        public Task DeleteByIdAsync(Guid roleId, Guid claimId)
        {
            return context.RoleClaims.DeleteAsync(rc => rc.RoleId == roleId && rc.ClaimId == claimId);
        }
    }
}