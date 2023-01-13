using Annium.Core.Mapper;
using Core.Domain.Entities;

namespace Server.Db.Profiles;

internal class CompanyUserProfile : Profile
{
    public CompanyUserProfile()
    {
        Map<CompanyUser, Entities.CompanyUser>().Ignore(x => x.User);
    }
}