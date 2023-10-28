using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.CompanyUsers;

namespace Server.Application.Commands.CompanyUsers;

internal class DeleteCompanyRoleFromCompanyUserCommandValidator : Validator<DeleteCompanyRoleFromCompanyUserCommand>
{
    public DeleteCompanyRoleFromCompanyUserCommandValidator()
    {
        Field(c => c.CompanyId).Required();
        Field(c => c.UserId).Required();
        Field(c => c.RoleId).Required();
    }
}

internal class DeleteCompanyRoleFromCompanyUserCommandComposer : Composer<DeleteCompanyRoleFromCompanyUserCommand>
{
    public DeleteCompanyRoleFromCompanyUserCommandComposer(
        ITokenAccessor tokenAccessor,
        ICompanyRepository companyRepository,
        IUserRepository userRepository,
        ICompanyRoleRepository roleRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Company).LoadWith(ctx => companyRepository.TryGetByIdAsync(ctx.Root.CompanyId));
        Field(c => c.User).LoadWith(ctx => userRepository.TryGetByIdAsync(ctx.Root.UserId));
        Field(c => c.Role).LoadWith(ctx => roleRepository.TryGetByIdAsync(ctx.Root.RoleId));
    }
}
