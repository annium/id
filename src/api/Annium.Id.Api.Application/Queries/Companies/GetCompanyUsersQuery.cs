using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Queries.Companies;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Companies;

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