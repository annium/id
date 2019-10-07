using System;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Db.Repositories.Implementations
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
            var entity = await context.CompanyRoleClaims
                .FirstOrDefaultAsync(x => x.RoleId == claim.RoleId && x.ClaimId == claim.ClaimId);

            if (entity is null)
            {
                entity = mapper.Map<Entities.CompanyRoleClaim>(claim);
                context.CompanyRoleClaims.Add(entity);
            }
            else
            {
                entity.Value = claim.Value;
            }

            await context.SaveChangesAsync();

            return mapper.Map<CompanyRoleClaim>(entity);
        }

        public async Task DeleteByIdAsync(Guid roleId, Guid claimId)
        {
            var entity = await context.CompanyRoleClaims
                .FirstOrDefaultAsync(x => x.RoleId == roleId && x.ClaimId == claimId);

            if (entity is null)
                return;

            context.CompanyRoleClaims.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}