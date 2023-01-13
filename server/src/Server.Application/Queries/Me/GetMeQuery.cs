using Annium.Extensions.Composition;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Queries.Me;

namespace Server.Application.Queries.Me;

internal class GetMeQueryComposer : Composer<GetMeQuery>
{
    public GetMeQueryComposer(
        ITokenAccessor tokenAccessor,
        IUserRepository userRepository
    )
    {
        Field(e => e.User).LoadWith(_ => userRepository.TryGetByIdAsync(tokenAccessor.GetToken().UserId));
    }
}