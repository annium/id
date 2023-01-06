using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Queries.Companies;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Companies;

internal class GetCompanyQueryValidator : Validator<GetCompanyQuery>
{
    public GetCompanyQueryValidator()
    {
        Field(c => c.CompanyId).Required();
    }
}

internal class GetCompanyQueryComposer : Composer<GetCompanyQuery>
{
    public GetCompanyQueryComposer(
        ICompanyRepository companyRepository
    )
    {
        Field(c => c.Company).LoadWith(ctx => companyRepository.GetByIdAsync(ctx.Root.CompanyId));
    }
}