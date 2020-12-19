using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Queries.Roles;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Roles
{
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
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}