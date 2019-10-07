using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Db.Profiles
{
    internal class CompanyRoleClaimProfile : Profile
    {
        public CompanyRoleClaimProfile()
        {
            Map<CompanyRoleClaim, Entities.CompanyRoleClaim>().Ignore(x => x.Role).Ignore(x => x.Claim);
            Map<Entities.CompanyRoleClaim, ClaimValue>(x => new ClaimValue(x.ClaimId, x.Claim.Key, x.Claim.Name, x.Value));
        }
    }
}