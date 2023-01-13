using System;
using Annium.Architecture.CQRS.Queries;
using Core.Domain.Entities;

namespace Server.Domain.Queries.Claims;

public class ListClaimsQuery : IQuery
{
    public Guid AppId { get; }
    public App App { get; private set; } = null!;

    public ListClaimsQuery(Guid appId)
    {
        AppId = appId;
    }
}