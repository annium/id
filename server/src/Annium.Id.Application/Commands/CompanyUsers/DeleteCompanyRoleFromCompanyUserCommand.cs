using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.CompanyUsers
{
    public class DeleteCompanyRoleFromCompanyUserCommand : ICommand
    {
        public Guid CompanyId { get; }
        public Guid UserId { get; }
        public Guid RoleId { get; }
        public Guid MyId { get; private set; }
        public Company Company { get; private set; }
        public User User { get; private set; }
        public CompanyRole Role { get; private set; }

        public DeleteCompanyRoleFromCompanyUserCommand(
            Guid companyId,
            Guid userId,
            Guid roleId
        )
        {
            CompanyId = companyId;
            UserId = userId;
            RoleId = roleId;
        }
    }

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
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.Company).LoadWith(ctx => companyRepository.GetByIdAsync(ctx.Root.CompanyId));
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
            Field(c => c.Role).LoadWith(ctx => roleRepository.GetByIdAsync(ctx.Root.RoleId));
        }
    }
}