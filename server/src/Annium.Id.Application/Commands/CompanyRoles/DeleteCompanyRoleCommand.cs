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
        public Guid AppId { get; }
        public Guid RoleId { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }
        public CompanyRole Role { get; private set; }

        public DeleteCompanyRoleCommand(
            Guid appId,
            Guid roleId
        )
        {
            AppId = appId;
            RoleId = roleId;
        }
    }

    internal class DeleteCompanyRoleCommandValidator : Validator<DeleteCompanyRoleCommand>
    {
        public DeleteCompanyRoleCommandValidator()
        {
            Field(c => c.AppId).NotEqual(Guid.Empty);
            Field(c => c.RoleId).NotEqual(Guid.Empty);
        }
    }

    internal class DeleteCompanyRoleCommandComposer : Composer<DeleteCompanyRoleCommand>
    {
        public DeleteCompanyRoleCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository,
            ICompanyRoleRepository companyRoleRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.Role).LoadWith(ctx => companyRoleRepository.GetByIdAsync(ctx.Root.RoleId));
        }
    }
}