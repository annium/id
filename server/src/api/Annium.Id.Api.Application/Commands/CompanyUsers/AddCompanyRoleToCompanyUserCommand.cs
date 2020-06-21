using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.CompanyUsers
{
    public class AddCompanyRoleToCompanyUserCommand : ICommand
    {
        public Guid CompanyId { get; }
        public Guid UserId { get; }
        public Guid RoleId { get; }
        public Guid MyId { get; private set; }
        public Company Company { get; private set; } = null!;
        public User User { get; private set; } = null!;
        public CompanyRole Role { get; private set; } = null!;

        public AddCompanyRoleToCompanyUserCommand(
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

    internal class AddCompanyRoleToCompanyUserCommandValidator : Validator<AddCompanyRoleToCompanyUserCommand>
    {
        public AddCompanyRoleToCompanyUserCommandValidator()
        {
            Field(c => c.CompanyId).Required();
            Field(c => c.UserId).Required();
            Field(c => c.RoleId).Required();
        }
    }

    internal class AddCompanyRoleToCompanyUserCommandComposer : Composer<AddCompanyRoleToCompanyUserCommand>
    {
        public AddCompanyRoleToCompanyUserCommandComposer(
            ITokenAccessor tokenAccessor,
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            ICompanyRoleRepository roleRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Company).LoadWith(ctx => companyRepository.GetByIdAsync(ctx.Root.CompanyId));
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
            Field(c => c.Role).LoadWith(ctx => roleRepository.GetByIdAsync(ctx.Root.RoleId));
        }
    }
}