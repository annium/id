using Annium.Core.Mapper;
using Annium.Id.Api.Application.Commands.Apps;
using Annium.Id.Api.ViewModels.Apps.Requests;

namespace Annium.Id.Api.ViewModels.Apps
{
    internal class AppProfile : Profile
    {
        public AppProfile()
        {
            Map<SetAppOwnerRequest, SetAppOwnerCommand>(r => new SetAppOwnerCommand(r.AppId, r.NewOwnerId));
        }
    }
}