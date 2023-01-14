using Annium.Core.Mapper;
using Server.Domain.Models;

namespace Server.Db.Profiles;

internal class CompanyRoleProfile : Profile
{
    public CompanyRoleProfile()
    {
        Map<CompanyRole, Entities.CompanyRole>().Ignore(x => x.Claims);
    }
}