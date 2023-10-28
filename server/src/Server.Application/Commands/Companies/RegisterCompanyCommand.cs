using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Companies;

namespace Server.Application.Commands.Companies;

internal class RegisterCompanyCommandValidator : Validator<RegisterCompanyCommand>
{
    public RegisterCompanyCommandValidator()
    {
        Field(c => c.ParentId).Required();
        Field(c => c.Name).Required().Length(3, 100);
    }
}

internal class RegisterCompanyCommandComposer : Composer<RegisterCompanyCommand>
{
    public RegisterCompanyCommandComposer(
        ITokenAccessor tokenAccessor,
        ICompanyRepository companyRepository,
        IUserRepository userRepository
    )
    {
        Field(c => c.Parent)
            .When(ctx => ctx.Root.ParentId.HasValue)
            .LoadWith(ctx => companyRepository.TryGetByIdAsync(ctx.Root.ParentId!.Value));
        Field(c => c.Me).LoadWith(_ => userRepository.TryGetByIdAsync(tokenAccessor.GetToken().UserId));
    }
}
