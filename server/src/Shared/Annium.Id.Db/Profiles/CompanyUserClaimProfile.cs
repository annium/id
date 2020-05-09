using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Db.Profiles
{
    internal class CompanyUserClaimProfile : Profile
    {
        public CompanyUserClaimProfile()
        {
            Map<CompanyUserClaim, Entities.CompanyUserClaim>().Ignore(x => x.Claim);
        }
    }
}