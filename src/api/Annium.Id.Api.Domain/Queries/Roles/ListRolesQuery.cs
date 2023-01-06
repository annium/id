using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Queries.Roles;

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