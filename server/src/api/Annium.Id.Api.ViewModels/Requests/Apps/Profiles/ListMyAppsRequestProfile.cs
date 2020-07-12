using Annium.Core.Mapper;
using Annium.Id.Api.Application.Queries.Apps;

namespace Annium.Id.Api.ViewModels.Requests.Apps.Profiles
{
    internal class SetAppOwnerRequestProfile : Profile
    {
        public SetAppOwnerRequestProfile()
        {
            Map<ListMyAppsRequest, ListMyAppsQuery>(r => new ListMyAppsQuery());
        }
    }
}