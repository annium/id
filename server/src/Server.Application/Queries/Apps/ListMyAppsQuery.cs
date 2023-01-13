using Annium.Extensions.Composition;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Queries.Apps;

namespace Server.Application.Queries.Apps;

internal class ListMyAppsQueryComposer : Composer<ListMyAppsQuery>
{
    public ListMyAppsQueryComposer(
        ITokenAccessor tokenAccessor,
        IUserRepository userRepository
    )
    {
        Field(c => c.User).LoadWith(_ => userRepository.TryGetByIdAsync(tokenAccessor.GetToken().UserId));
    }
}