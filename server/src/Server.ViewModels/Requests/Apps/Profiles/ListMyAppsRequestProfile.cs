using Annium.Core.Mapper;
using Server.Domain.Queries.Apps;

namespace Server.ViewModels.Requests.Apps.Profiles;

internal class ListMyAppsRequestProfile : Profile
{
    public ListMyAppsRequestProfile()
    {
        Map<ListMyAppsRequest, ListMyAppsQuery>(r => new ListMyAppsQuery());
    }
}
