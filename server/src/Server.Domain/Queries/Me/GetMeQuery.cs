using Annium.Architecture.CQRS.Queries;
using Server.Domain.Models;

namespace Server.Domain.Queries.Me;

public class GetMeQuery : IQuery
{
    public User User { get; private set; } = null!;
}