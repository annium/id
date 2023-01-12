using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Companies;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Companies;

internal class UpdateCompanyCommandValidator : Validator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        Field(c => c.CompanyId).Required();
        Field(c => c.ParentId).Required();
        Field(c => c.Name).Required().Length(3, 100);
    }
}

internal class UpdateCompanyCommandComposer : Composer<UpdateCompanyCommand>
{
    public UpdateCompanyCommandComposer(
        ITokenAccessor tokenAccessor,
        ICompanyRepository companyRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Company).LoadWith(ctx => companyRepository.TryGetByIdAsync(ctx.Root.CompanyId));
    }
}