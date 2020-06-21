using Annium.Core.Mapper;
using Annium.Id.Api.Application.Commands.Me;
using Annium.Id.Api.Application.Queries.Me;
using Annium.Id.Api.ViewModels.Me.Requests;

namespace Annium.Id.Api.ViewModels.Users
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