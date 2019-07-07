using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Extensions.Mapper;
using LinqToDB;

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
            var entity = mapper.Map<Entities.UserClaim>(claim);

            using(var db = context.GetDataConnection())
            {
                await db.InsertOrReplaceAsync(entity);
            }

            return mapper.Map<UserClaim>(entity);
        }

        public async Task<ClaimValue[]> GetUserClaimsAsync(Guid appId, Guid userId)
        {
            var raw = await context.Claims
                .InnerJoin(context.UserClaims, (c, uc) => uc.ClaimId == c.Id, (c, uc) => new { c, uc })
                .Where(e => e.c.AppId == appId && e.uc.UserId == userId)
                .ToArrayAsync();

            var claims = raw
                .GroupBy(e => e.c)
                .Select(g =>
                {
                    var c = g.Key;

                    return new Entities.ClaimValue { Id = c.Id, Key = c.Key, Name = c.Name, Value = g.First().uc.Value };
                })
                .ToArray();

            return claims.Select(mapper.Map<ClaimValue>).ToArray();
        }

        public Task DeleteByIdAsync(Guid userId, Guid claimId)
        {
            return context.UserClaims.DeleteAsync(uc => uc.UserId == userId && uc.ClaimId == claimId);
        }
    }
}