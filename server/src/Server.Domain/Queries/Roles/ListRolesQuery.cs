using System;
using Annium.Architecture.CQRS.Queries;
using Server.Domain.Models;

namespace Server.Domain.Queries.Roles;

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