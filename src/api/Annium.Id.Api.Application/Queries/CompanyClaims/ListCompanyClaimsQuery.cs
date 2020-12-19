using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Queries.CompanyClaims;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.CompanyClaims
{
    internal class ListCompanyClaimsQueryValidator : Validator<ListCompanyClaimsQuery>
    {
        public ListCompanyClaimsQueryValidator()
        {
            Field(c => c.AppId).Required();
        }
    }

    internal class ListCompanyClaimsQueryComposer : Composer<ListCompanyClaimsQuery>
    {
        public ListCompanyClaimsQueryComposer(
            IAppRepository appRepository
        )
        {
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}