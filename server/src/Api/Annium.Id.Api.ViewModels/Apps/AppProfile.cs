using Annium.Core.Mapper;
using Annium.Id.Application.Commands.Apps;
using Annium.Id.ViewModels.Apps.Requests;

namespace Annium.Id.ViewModels.Apps
{
    internal class AppProfile : Profile
    {
        public AppProfile()
        {
            Map<SetAppOwnerRequest, SetAppOwnerCommand>(r => new SetAppOwnerCommand(r.AppId, r.NewOwnerId));
        }
    }
}