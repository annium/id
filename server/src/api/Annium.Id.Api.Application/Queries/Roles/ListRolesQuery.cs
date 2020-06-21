using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Roles
{
    public class ListRolesQuery : IQuery
    {
        public Guid AppId { get; }
        public App App { get; private set; } = null!;

        public ListRolesQuery(
            Guid appId
        )
        {
            AppId = appId;
        }
    }

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