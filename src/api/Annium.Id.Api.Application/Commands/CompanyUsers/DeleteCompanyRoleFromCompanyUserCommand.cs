using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.CompanyUsers;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.CompanyUsers
{
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
            Field(c => c.Company).LoadWith(ctx => companyRepository.GetByIdAsync(ctx.Root.CompanyId));
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
            Field(c => c.Role).LoadWith(ctx => roleRepository.GetByIdAsync(ctx.Root.RoleId));
        }
    }
}