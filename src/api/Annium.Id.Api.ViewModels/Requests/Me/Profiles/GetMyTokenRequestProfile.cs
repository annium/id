using Annium.Core.Mapper;
using Annium.Id.Api.Domain.Queries.Me;

namespace Annium.Id.Api.ViewModels.Requests.Me.Profiles;

internal class GetMyTokenRequestProfile : Profile
{
    public GetMyTokenRequestProfile()
    {
        Map<GetMyTokenRequest, GetMyTokenQuery>(r => new GetMyTokenQuery());
    }
}