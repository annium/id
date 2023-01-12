using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Companies;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Companies;

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