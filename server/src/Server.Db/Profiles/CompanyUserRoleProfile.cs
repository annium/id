using Annium.Core.Mapper;
using Core.Domain.Entities;

namespace Server.Db.Profiles;

internal class CompanyUserRoleProfile : Profile
{
    public CompanyUserRoleProfile()
    {
        Map<CompanyUserRole, Entities.CompanyUserRole>().Ignore(x => x.Role);
    }
}