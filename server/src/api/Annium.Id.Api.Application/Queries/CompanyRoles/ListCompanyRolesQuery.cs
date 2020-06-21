using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.CompanyRoles
{
    public class ListCompanyRolesQuery : IQuery
    {
        public Guid AppId { get; }
        public App App { get; private set; } = null!;

        public ListCompanyRolesQuery(
            Guid appId
        )
        {
            AppId = appId;
        }
    }

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
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}