using Annium.Architecture.CQRS.Queries;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Queries.Me
{
    public class GetMyTokenQuery : IQuery
    {
        public UserLogin Login { get; private set; } = default!;
    }
}