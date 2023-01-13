using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Server.Db.Repositories;
using Server.Domain.Queries.Roles;

namespace Server.Application.Queries.Roles;

internal class ListRolesQueryValidator : Validator<ListRolesQuery>
{
    public ListRolesQueryValidator()
    {
        Field(c => c.AppId).Required();
    }
}

internal class ListRolesQueryComposer : Composer<ListRolesQuery>
{
    public ListRolesQueryComposer(
        IAppRepository appRepository
    )
    {
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}