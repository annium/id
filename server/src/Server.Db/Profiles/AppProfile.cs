using Annium.Core.Mapper;
using Core.Domain.Entities;

namespace Server.Db.Profiles;

internal class AppProfile : Profile
{
    public AppProfile()
    {
        Map<App, Entities.App>().Ignore(x => x.Owner);
    }
}