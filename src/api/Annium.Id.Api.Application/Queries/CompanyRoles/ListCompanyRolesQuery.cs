using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Queries.CompanyRoles;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.CompanyRoles;

internal class ListCompanyRolesQueryValidator : Validator<ListCompanyRolesQuery>
{
    public ListCompanyRolesQueryValidator()
    {
        Field(c => c.AppId).Required();
    }
}

internal class ListCompanyRolesQueryComposer : Composer<ListCompanyRolesQuery>
{
    public ListCompanyRolesQueryComposer(
        IAppRepository appRepository
    )
    {
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}