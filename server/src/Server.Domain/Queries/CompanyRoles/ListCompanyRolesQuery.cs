using System;
using Annium.Architecture.CQRS.Queries;
using Server.Domain.Models;

namespace Server.Domain.Queries.CompanyRoles;

public class ListCompanyRolesQuery : IQuery
{
    public Guid AppId { get; }
    public App App { get; private set; } = null!;

    public ListCompanyRolesQuery(Guid appId)
    {
        AppId = appId;
    }
}
