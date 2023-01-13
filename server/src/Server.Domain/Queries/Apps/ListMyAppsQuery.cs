using Annium.Architecture.CQRS.Queries;
using Core.Domain.Entities;

namespace Server.Domain.Queries.Apps;

public class ListMyAppsQuery : IQuery
{
    public User User { get; private set; } = default!;
}