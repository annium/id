using System;
using Annium.Architecture.CQRS.Queries;
using Core.Domain.Entities;

namespace Server.Domain.Queries.Users;

public class GetUserQuery : IQuery
{
    public Guid UserId { get; }
    public User User { get; private set; } = default!;

    public GetUserQuery(
        Guid userId
    )
    {
        UserId = userId;
    }
}