using Annium.Core.Mapper;
using Annium.Id.Api.Application.Queries.Apps;

namespace Annium.Id.Api.ViewModels.Requests.Apps.Profiles
{
    internal class ListMyAppsRequestProfile : Profile
    {
        public ListMyAppsRequestProfile()
        {
            Map<ListMyAppsRequest, ListMyAppsQuery>(r => new ListMyAppsQuery());
        }
    }
}