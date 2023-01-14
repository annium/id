using System.Linq;
using Annium.Core.Mapper;
using Server.Domain.Models;

namespace Server.Db.Profiles;

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