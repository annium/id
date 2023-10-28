using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.CompanyUsers;

namespace Server.Application.Commands.CompanyUsers;

internal class AddUserToCompanyCommandValidator : Validator<AddUserToCompanyCommand>
{
    public AddUserToCompanyCommandValidator()
    {
        Field(c => c.CompanyId).Required();
        Field(c => c.UserId).Required();
    }
}

internal class AddUserToCompanyCommandComposer : Composer<AddUserToCompanyCommand>
{
    public AddUserToCompanyCommandComposer(
        ITokenAccessor tokenAccessor,
        ICompanyRepository companyRepository,
        IUserRepository userRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Company).LoadWith(ctx => companyRepository.TryGetByIdAsync(ctx.Root.CompanyId));
        Field(c => c.User).LoadWith(ctx => userRepository.TryGetByIdAsync(ctx.Root.UserId));
    }
}
