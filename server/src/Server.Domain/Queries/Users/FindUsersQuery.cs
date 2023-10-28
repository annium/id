using Annium.Architecture.CQRS.Queries;

namespace Server.Domain.Queries.Users;

public class FindUsersQuery : IQuery
{
    public string Query { get; }
    public int Limit { get; }

    public FindUsersQuery(string query, int limit)
    {
        Query = query;
        Limit = limit;
    }
}
