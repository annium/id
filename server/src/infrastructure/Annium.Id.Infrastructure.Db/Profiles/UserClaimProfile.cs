using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Profiles
{
    internal class UserClaimProfile : Profile
    {
        public UserClaimProfile()
        {
            Map<UserClaim, Entities.UserClaim>().Ignore(x => x.Claim);
            Map<Entities.UserClaim, ClaimValue>(x => new ClaimValue(x.ClaimId, x.Claim.Key, x.Claim.Name, x.Value));
        }
    }
}