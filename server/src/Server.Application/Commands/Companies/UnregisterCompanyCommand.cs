using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Companies;

namespace Server.Application.Commands.Companies;

internal class UnregisterCompanyCommandValidator : Validator<UnregisterCompanyCommand>
{
    public UnregisterCompanyCommandValidator()
    {
        Field(c => c.CompanyId).Required();
    }
}

internal class UnregisterCompanyCommandComposer : Composer<UnregisterCompanyCommand>
{
    public UnregisterCompanyCommandComposer(
        ITokenAccessor tokenAccessor,
        ICompanyRepository companyRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Company).LoadWith(ctx => companyRepository.TryGetByIdAsync(ctx.Root.CompanyId));
    }
}