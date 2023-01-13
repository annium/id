using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Domain.Commands.Companies;

namespace Server.Application.Commands.Companies;

internal class RegisterCompanyCommandValidator : Validator<RegisterCompanyCommand>
{
    public RegisterCompanyCommandValidator(
    )
    {
        Field(c => c.ParentId).Required();
        Field(c => c.Name).Required().Length(3, 100);
    }
}

internal class RegisterCompanyCommandComposer : Composer<RegisterCompanyCommand>
{
    public RegisterCompanyCommandComposer(
        ITokenAccessor tokenAccessor
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
    }
}