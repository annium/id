using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Server.Db.Repositories;
using Server.Domain.Queries.CompanyClaims;

namespace Server.Application.Queries.CompanyClaims;

internal class ListCompanyClaimsQueryValidator : Validator<ListCompanyClaimsQuery>
{
    public ListCompanyClaimsQueryValidator()
    {
        Field(c => c.AppId).Required();
    }
}

internal class ListCompanyClaimsQueryComposer : Composer<ListCompanyClaimsQuery>
{
    public ListCompanyClaimsQueryComposer(IAppRepository appRepository)
    {
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}
