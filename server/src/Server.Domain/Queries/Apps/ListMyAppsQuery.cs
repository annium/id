using Annium.Architecture.CQRS.Queries;
using Server.Domain.Models;

namespace Server.Domain.Queries.Apps;

public class ListMyAppsQuery : IQuery
{
    public User User { get; private set; } = default!;
}
