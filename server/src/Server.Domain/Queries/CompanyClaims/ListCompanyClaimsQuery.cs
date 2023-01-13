using System;
using Annium.Architecture.CQRS.Queries;
using Core.Domain.Entities;

namespace Server.Domain.Queries.CompanyClaims;

public class ListCompanyClaimsQuery : IQuery
{
    public Guid AppId { get; }
    public App App { get; private set; } = null!;

    public ListCompanyClaimsQuery(Guid appId)
    {
        AppId = appId;
    }
}