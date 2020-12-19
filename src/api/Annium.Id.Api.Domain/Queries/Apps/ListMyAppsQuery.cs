using Annium.Architecture.CQRS.Queries;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Queries.Apps
{
    public class ListMyAppsQuery : IQuery
    {
        public User User { get; private set; } = default!;
    }
}