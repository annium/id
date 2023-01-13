using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Server.Db.Repositories;
using Server.Domain.Queries.Companies;

namespace Server.Application.Queries.Companies;

internal class GetCompanyUsersQueryValidator : Validator<GetCompanyUsersQuery>
{
    public GetCompanyUsersQueryValidator()
    {
        Field(c => c.CompanyId).Required();
    }
}

internal class GetCompanyUsersQueryComposer : Composer<GetCompanyUsersQuery>
{
    public GetCompanyUsersQueryComposer(
        ICompanyRepository companyRepository
    )
    {
        Field(c => c.Company).LoadWith(ctx => companyRepository.TryGetByIdAsync(ctx.Root.CompanyId));
    }
}