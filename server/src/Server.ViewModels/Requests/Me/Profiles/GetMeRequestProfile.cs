using Annium.Core.Mapper;
using Server.Domain.Queries.Me;

namespace Server.ViewModels.Requests.Me.Profiles;

internal class GetMeRequestProfile : Profile
{
    public GetMeRequestProfile()
    {
        Map<GetMeRequest, GetMeQuery>(r => new GetMeQuery());
    }
}
