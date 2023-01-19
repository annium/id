using Annium.Extensions.Validation;
using Server.Domain.Queries.Companies;

namespace Server.Application.Queries.Companies;

internal class FindCompaniesQueryValidator : Validator<FindCompaniesQuery>
{
    public FindCompaniesQueryValidator(
    )
    {
        Field(c => c.Query).Length(2, 100, "Company query length must be between 2 and 100 characters long");
    }
}