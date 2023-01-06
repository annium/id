using System.Linq;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Profiles;

internal class UserRoleProfile : Profile
{
    public UserRoleProfile()
    {
        Map<UserRole, Entities.UserRole>().Ignore(x => x.Role);
        Map<Entities.UserRole, Role>(x => new Role(
            x.Role.AppId,
            x.Role.Key,
            x.Role.Name,
            x.Role.Claims.Select(y => new ClaimValue(y.ClaimId, y.Claim.Key, y.Claim.Name, y.Value)).ToArray()
        ));
    }
}