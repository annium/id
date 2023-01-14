using Annium.Architecture.CQRS.Queries;
using Server.Domain.Models;

namespace Server.Domain.Queries.Me;

public class GetMyTokenQuery : IQuery
{
    public UserLogin Login { get; private set; } = default!;
}