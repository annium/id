using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.CompanyUsers;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.CompanyUsers;

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