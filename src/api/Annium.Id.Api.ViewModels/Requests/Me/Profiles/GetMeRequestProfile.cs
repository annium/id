using Annium.Core.Mapper;
using Annium.Id.Api.Domain.Queries.Me;

namespace Annium.Id.Api.ViewModels.Requests.Me.Profiles
{
    internal class GetMeRequestProfile : Profile
    {
        public GetMeRequestProfile()
        {
            Map<GetMeRequest, GetMeQuery>(r => new GetMeQuery());
        }
    }
}