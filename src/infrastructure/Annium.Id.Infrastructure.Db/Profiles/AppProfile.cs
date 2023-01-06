using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Profiles;

internal class AppProfile : Profile
{
    public AppProfile()
    {
        Map<App, Entities.App>().Ignore(x => x.Owner);
    }
}