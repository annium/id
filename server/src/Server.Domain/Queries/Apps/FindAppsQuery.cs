using Annium.Architecture.CQRS.Queries;

namespace Server.Domain.Queries.Apps;

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