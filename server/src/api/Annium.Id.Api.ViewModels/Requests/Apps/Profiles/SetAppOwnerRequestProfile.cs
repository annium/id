using Annium.Core.Mapper;
using Annium.Id.Api.Application.Commands.Apps;

namespace Annium.Id.Api.ViewModels.Requests.Apps.Profiles
{
    internal class SetAppOwnerRequestProfile : Profile
    {
        public SetAppOwnerRequestProfile()
        {
            Map<SetAppOwnerRequest, SetAppOwnerCommand>(r => new SetAppOwnerCommand(r.AppId, r.NewOwnerId));
        }
    }
}