using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Db.Profiles
{
    internal class CompanyUserRoleProfile : Profile
    {
        public CompanyUserRoleProfile()
        {
            Map<CompanyUserRole, Entities.CompanyUserRole>().Ignore(x => x.Role);
        }
    }
}