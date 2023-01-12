using Annium.Extensions.Composition;
using Annium.Id.Api.Domain.Queries.Apps;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Apps;

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