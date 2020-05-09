using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Queries.CompanyClaims
{
    public class ListCompanyClaimsQuery : IQuery
    {
        public Guid AppId { get; }
        public App App { get; private set; } = null!;

        public ListCompanyClaimsQuery(Guid appId)
        {
            AppId = appId;
        }
    }

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