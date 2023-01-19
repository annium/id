using Annium.Extensions.Validation;
using Server.Domain.Queries.Apps;

namespace Server.Application.Queries.Apps;

internal class FindAppsQueryValidator : Validator<FindAppsQuery>
{
    public FindAppsQueryValidator(
    )
    {
        Field(c => c.Query).MaxLength(100, "App query length must be between max 100 characters long");
    }
}