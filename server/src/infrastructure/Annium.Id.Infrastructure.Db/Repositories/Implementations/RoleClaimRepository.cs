using System;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Infrastructure.Db.Repositories.Implementations
{
    internal class RoleClaimRepository : IRoleClaimRepository
    {
        private readonly IContext context;
        private readonly IMapper mapper;

        public RoleClaimRepository(
            IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<RoleClaim> SaveAsync(RoleClaim claim)
        {
            var entity = await context.RoleClaims
                .FirstOrDefaultAsync(x => x.RoleId == claim.RoleId && x.ClaimId == claim.ClaimId);

            if (entity is null)
            {
                entity = mapper.Map<Entities.RoleClaim>(claim);
                context.RoleClaims.Add(entity);
            }
            else
            {
                entity.Value = claim.Value;
            }

            await context.SaveChangesAsync();

            return mapper.Map<RoleClaim>(entity);
        }

        public async Task DeleteByIdAsync(Guid roleId, Guid claimId)
        {
            var entity = await context.RoleClaims
                .FirstOrDefaultAsync(x => x.RoleId == roleId && x.ClaimId == claimId);

            if (entity is null)
                return;

            context.RoleClaims.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}