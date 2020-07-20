using Annium.Architecture.CQRS.Queries;

namespace Annium.Id.Api.Application.Queries.Apps
{
    public class FindAppsQuery : IQuery
    {
        public string Query { get; }

        public FindAppsQuery(
            string query
        )
        {
            Query = query;
        }
    }
}