using Annium.Core.Mapper;
using Server.Domain.Commands.Me;

namespace Server.ViewModels.Requests.Me.Profiles;

internal class UnregisterMeRequestProfile : Profile
{
    public UnregisterMeRequestProfile()
    {
        Map<UnregisterMeRequest, UnregisterMeCommand>(r => new UnregisterMeCommand());
    }
}
