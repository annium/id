using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Db.Profiles
{
    internal class RoleProfile : Profile
    {
        public RoleProfile()
        {
            Map<Role, Entities.Role>().Ignore(x => x.Claims);
        }
    }
}