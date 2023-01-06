using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Profiles;

internal class CompanyRoleProfile : Profile
{
    public CompanyRoleProfile()
    {
        Map<CompanyRole, Entities.CompanyRole>().Ignore(x => x.Claims);
    }
}