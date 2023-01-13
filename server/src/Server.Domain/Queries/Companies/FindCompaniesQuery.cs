using Annium.Architecture.CQRS.Queries;

namespace Server.Domain.Queries.Companies;

public class FindCompaniesQuery : IQuery
{
    public string Query { get; }

    public FindCompaniesQuery(
        string query
    )
    {
        Query = query;
    }
}