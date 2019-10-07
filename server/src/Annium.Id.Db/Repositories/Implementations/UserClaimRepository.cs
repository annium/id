using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class UserClaimRepository : IUserClaimRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public UserClaimRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<UserClaim> SaveAsync(UserClaim claim)
        {
            var entity = await context.UserClaims
                .FirstOrDefaultAsync(x => x.UserId == claim.UserId && x.ClaimId == claim.ClaimId);

            if (entity is null)
            {
                entity = mapper.Map<Entities.UserClaim>(claim);
                context.UserClaims.Add(entity);
            }
            else
            {
                entity.Value = claim.Value;
            }

            await context.SaveChangesAsync();

            return mapper.Map<UserClaim>(entity);
        }

        public async Task<ClaimValue[]> GetUserClaimsAsync(Guid appId, Guid userId)
        {
            var raw = await context.UserClaims.AsNoTracking()
                .Include(x => x.Claim)
                .Where(x => x.Claim.AppId == appId && x.UserId == userId)
                .ToListAsync();

            return raw.Select(mapper.Map<ClaimValue>).ToArray();
        }

        public async Task DeleteByIdAsync(Guid userId, Guid claimId)
        {
            var entity = await context.UserClaims
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ClaimId == claimId);

            if (entity is null)
                return;

            context.UserClaims.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}