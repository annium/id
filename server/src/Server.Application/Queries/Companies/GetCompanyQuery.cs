using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Server.Db.Repositories;
using Server.Domain.Queries.Companies;

namespace Server.Application.Queries.Companies;

internal class GetCompanyQueryValidator : Validator<GetCompanyQuery>
{
    public GetCompanyQueryValidator()
    {
        Field(c => c.CompanyId).Required();
    }
}

internal class GetCompanyQueryComposer : Composer<GetCompanyQuery>
{
    public GetCompanyQueryComposer(ICompanyRepository companyRepository)
    {
        Field(c => c.Company).LoadWith(ctx => companyRepository.TryGetByIdAsync(ctx.Root.CompanyId));
    }
}
