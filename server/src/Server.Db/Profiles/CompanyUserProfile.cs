using Annium.Core.Mapper;
using Server.Domain.Models;

namespace Server.Db.Profiles;

internal class CompanyUserProfile : Profile
{
    public CompanyUserProfile()
    {
        Map<CompanyUser, Entities.CompanyUser>().Ignore(x => x.User);
    }
}