using Annium.Core.Mapper;
using Core.Domain.Entities;

namespace Server.Db.Profiles;

internal class CompanyRoleProfile : Profile
{
    public CompanyRoleProfile()
    {
        Map<CompanyRole, Entities.CompanyRole>().Ignore(x => x.Claims);
    }
}