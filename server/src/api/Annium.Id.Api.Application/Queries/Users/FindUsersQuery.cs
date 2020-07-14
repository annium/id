using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Validation;

namespace Annium.Id.Api.Application.Queries.Users
{
    public class FindUsersQuery : IQuery
    {
        public string Query { get; }
        public int Limit { get; }

        public FindUsersQuery(
            string query,
            int limit
        )
        {
            Query = query;
            Limit = limit;
        }
    }

    internal class FindUsersQueryValidator : Validator<FindUsersQuery>
    {
        public FindUsersQueryValidator(
        )
        {
            Field(c => c.Query).Length(3, 20, "User query length must be between 3 and 20 characters long");
            Field(c => c.Limit).Between(1, 10,"User query limit must return 1 to 10 users");
        }
    }
}