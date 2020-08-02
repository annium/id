using Annium.Architecture.CQRS.Queries;

namespace Annium.Id.Api.Application.Queries.Companies
{
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
}