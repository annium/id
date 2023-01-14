using Annium.Core.Mapper;
using Server.Domain.Models;

namespace Server.Db.Profiles;

internal class RoleProfile : Profile
{
    public RoleProfile()
    {
        Map<Role, Entities.Role>().Ignore(x => x.Claims);
    }
}