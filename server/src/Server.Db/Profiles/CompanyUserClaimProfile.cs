using Annium.Core.Mapper;
using Server.Domain.Models;

namespace Server.Db.Profiles;

internal class CompanyUserClaimProfile : Profile
{
    public CompanyUserClaimProfile()
    {
        Map<CompanyUserClaim, Entities.CompanyUserClaim>().Ignore(x => x.Claim);
    }
}