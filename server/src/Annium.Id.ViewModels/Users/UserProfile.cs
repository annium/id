using Annium.Core.Mapper;
using Annium.Id.Application.Commands.Me;
using Annium.Id.Application.Queries.Me;
using Annium.Id.ViewModels.Me.Requests;

namespace Annium.Id.ViewModels.Users
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            Map<UnregisterMeRequest, UnregisterMeCommand>(r => new UnregisterMeCommand());
            Map<GetMeRequest, GetMeQuery>(r => new GetMeQuery());
        }
    }
}