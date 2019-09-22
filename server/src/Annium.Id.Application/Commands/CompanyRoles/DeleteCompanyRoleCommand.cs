using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.CompanyRoles
{
    public class DeleteCompanyRoleCommand : ICommand
    {
        public Guid RoleId { get; }
        public Guid MyId { get; private set; }
        public CompanyRole Role { get; private set; }

        public DeleteCompanyRoleCommand(
            Guid roleId
        )
        {
            RoleId = roleId;
        }
    }

    internal class DeleteCompanyRoleCommandValidator : Validator<DeleteCompanyRoleCommand>
    {
        public DeleteCompanyRoleCommandValidator()
        {
            Field(c => c.RoleId).Required();
        }
    }

    internal class DeleteCompanyRoleCommandComposer : Composer<DeleteCompanyRoleCommand>
    {
        public DeleteCompanyRoleCommandComposer(
            ITokenAccessor tokenAccessor,
            ICompanyRoleRepository companyRoleRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.Role).LoadWith(ctx => companyRoleRepository.GetByIdAsync(ctx.Root.RoleId));
        }
    }
}