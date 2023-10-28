using Annium.Core.Mapper;
using Server.Domain.Queries.Me;

namespace Server.ViewModels.Requests.Me.Profiles;

internal class GetMyTokenRequestProfile : Profile
{
    public GetMyTokenRequestProfile()
    {
        Map<GetMyTokenRequest, GetMyTokenQuery>(r => new GetMyTokenQuery());
    }
}
