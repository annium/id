using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Queries.Me;

namespace Server.Application.Queries.Me;

internal class GetTokenQueryValidator : Validator<GetMyTokenQuery> { }

internal class GetTokenQueryComposer : Composer<GetMyTokenQuery>
{
    public GetTokenQueryComposer(ITokenAccessor tokenAccessor, IUserLoginRepository userLoginRepository)
    {
        Field(e => e.Login).LoadWith(_ => userLoginRepository.TryGetByIdAsync(tokenAccessor.GetToken().LoginId));
    }
}
