using System;
using Annium.Architecture.CQRS.Queries;
using Server.Domain.Models;

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