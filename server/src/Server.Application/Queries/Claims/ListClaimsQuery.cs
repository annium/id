using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Server.Db.Repositories;
using Server.Domain.Queries.Claims;

namespace Server.Application.Queries.Claims;

internal class ListClaimsQueryValidator : Validator<ListClaimsQuery>
{
    public ListClaimsQueryValidator()
    {
        Field(c => c.AppId).Required();
    }
}

internal class ListClaimsQueryComposer : Composer<ListClaimsQuery>
{
    public ListClaimsQueryComposer(IAppRepository appRepository)
    {
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}
