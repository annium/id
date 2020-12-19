using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Queries.Users;

namespace Annium.Id.Api.Application.Queries.Users
{
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