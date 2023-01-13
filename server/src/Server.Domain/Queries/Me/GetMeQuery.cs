using Annium.Architecture.CQRS.Queries;
using Core.Domain.Entities;

namespace Server.Domain.Queries.Me;

public class GetMeQuery : IQuery
{
    public User User { get; private set; } = null!;
}