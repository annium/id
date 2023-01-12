using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Queries.Me;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Me;

internal class GetTokenQueryValidator : Validator<GetMyTokenQuery>
{
}

internal class GetTokenQueryComposer : Composer<GetMyTokenQuery>
{
    public GetTokenQueryComposer(
        ITokenAccessor tokenAccessor,
        IUserLoginRepository userLoginRepository
    )
    {
        Field(e => e.Login).LoadWith(_ => userLoginRepository.TryGetByIdAsync(tokenAccessor.GetToken().LoginId));
    }
}