using Annium.Core.Mapper;
using Annium.Id.Api.Application.Commands.Me;

namespace Annium.Id.Api.ViewModels.Requests.Me.Profiles
{
    internal class UnregisterMeRequestProfile : Profile
    {
        public UnregisterMeRequestProfile()
        {
            Map<UnregisterMeRequest, UnregisterMeCommand>(r => new UnregisterMeCommand());
        }
    }
}