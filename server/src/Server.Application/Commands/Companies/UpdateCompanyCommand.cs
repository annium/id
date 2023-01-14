using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Companies;

namespace Server.Application.Commands.Companies;

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
        Field(c => c.Parent)
            .When(ctx => ctx.Root.ParentId.HasValue)
            .LoadWith(ctx => companyRepository.TryGetByIdAsync(ctx.Root.ParentId!.Value));
    }
}