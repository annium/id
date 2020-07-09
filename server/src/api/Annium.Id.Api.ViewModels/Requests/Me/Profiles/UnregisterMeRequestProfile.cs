using Annium.Core.Mapper;
using Annium.Id.Api.Application.Commands.Me;
using Annium.Id.Api.Application.Queries.Me;

namespace Annium.Id.Api.ViewModels.Requests.Me.Profiles
{
    internal class UnregisterMeRequestProfile : Profile
    {
        public UnregisterMeRequestProfile()
        {
            Map<UnregisterMeRequest, UnregisterMeCommand>(r => new UnregisterMeCommand());
            Map<GetMeRequest, GetMeQuery>(r => new GetMeQuery());
        }
    }
}