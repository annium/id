using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Queries.CompanyRoles
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
}