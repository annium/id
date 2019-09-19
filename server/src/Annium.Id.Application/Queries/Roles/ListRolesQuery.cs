using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Queries.Roles
{
    public class ListRolesQuery : IQuery
    {
        public Guid AppId { get; }
        public App App { get; private set; }

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
            Field(c => c.AppId);
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