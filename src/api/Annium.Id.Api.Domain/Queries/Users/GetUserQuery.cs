using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Queries.Users
{
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
}