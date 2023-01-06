using Annium.Extensions.Composition;
using Annium.Id.Api.Domain.Queries.Me;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Me;

internal class GetMeQueryComposer : Composer<GetMeQuery>
{
    public GetMeQueryComposer(
        ITokenAccessor tokenAccessor,
        IUserRepository userRepository
    )
    {
        Field(e => e.User).LoadWith(ctx => userRepository.GetByIdAsync(tokenAccessor.GetToken().UserId));
    }
}