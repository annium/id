using Annium.Core.Mapper;
using Server.Domain.Models;

namespace Server.Db.Profiles;

internal class UserRoleProfile : Profile
{
    public UserRoleProfile()
    {
        Map<UserRole, Entities.UserRole>().Ignore(x => x.Role);
        Map<Entities.UserRole, Role>(x => new Role(
            null!,
            x.Role.Key,
            x.Role.Name
        ));
    }
}