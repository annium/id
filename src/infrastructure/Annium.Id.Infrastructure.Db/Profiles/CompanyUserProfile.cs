using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Profiles
{
    internal class CompanyUserProfile : Profile
    {
        public CompanyUserProfile()
        {
            Map<CompanyUser, Entities.CompanyUser>().Ignore(x => x.User);
        }
    }
}