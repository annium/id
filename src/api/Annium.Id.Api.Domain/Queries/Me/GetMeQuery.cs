using Annium.Architecture.CQRS.Queries;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Queries.Me
{
    public class GetMeQuery : IQuery
    {
        public User User { get; private set; } = null!;
    }
}