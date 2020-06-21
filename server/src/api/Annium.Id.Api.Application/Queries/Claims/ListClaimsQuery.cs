using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Claims
{
    public class ListClaimsQuery : IQuery
    {
        public Guid AppId { get; }
        public App App { get; private set; } = null!;

        public ListClaimsQuery(Guid appId)
        {
            AppId = appId;
        }
    }

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