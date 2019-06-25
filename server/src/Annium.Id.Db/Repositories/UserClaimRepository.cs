using System;
using System.Threading.Tasks;
using Annium.Extensions.Mapper;
using LinqToDB;

namespace Annium.Id.Db
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

        public Task DeleteByIdAsync(Guid userId, Guid claimId)
        {
            return context.UserClaims.DeleteAsync(uc => uc.UserId == userId && uc.ClaimId == claimId);
        }
    }
}