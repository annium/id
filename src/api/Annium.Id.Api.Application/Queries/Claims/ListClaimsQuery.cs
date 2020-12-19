using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Queries.Claims;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Claims
{
    internal class ListClaimsQueryValidator : Validator<ListClaimsQuery>
    {
        public ListClaimsQueryValidator()
        {
            Field(c => c.AppId).Required();
        }
    }

    internal class ListClaimsQueryComposer : Composer<ListClaimsQuery>
    {
        public ListClaimsQueryComposer(
            IAppRepository appRepository
        )
        {
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}