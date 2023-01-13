using Annium.Architecture.CQRS.Queries;
using Core.Domain.Entities;

namespace Server.Domain.Queries.Me;

public class GetMyTokenQuery : IQuery
{
    public UserLogin Login { get; private set; } = default!;
}