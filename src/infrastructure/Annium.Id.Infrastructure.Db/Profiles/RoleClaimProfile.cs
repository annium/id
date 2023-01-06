using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Profiles;

internal class RoleClaimProfile : Profile
{
    public RoleClaimProfile()
    {
        Map<RoleClaim, Entities.RoleClaim>().Ignore(x => x.Role).Ignore(x => x.Claim);
        Map<Entities.RoleClaim, ClaimValue>(x => new ClaimValue(x.ClaimId, x.Claim.Key, x.Claim.Name, x.Value));
    }
}