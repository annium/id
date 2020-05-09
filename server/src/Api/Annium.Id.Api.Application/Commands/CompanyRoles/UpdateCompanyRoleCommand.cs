using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.CompanyRoles
{
    public class UpdateCompanyRoleCommand : ICommand
    {
        public Guid RoleId { get; }
        public string Key { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }
        public CompanyRole Role { get; private set; } = null!;

        public UpdateCompanyRoleCommand(
            Guid roleId,
            string key,
            string name
        )
        {
            RoleId = roleId;
            Key = key;
            Name = name;
        }
    }

    internal class UpdateCompanyRoleCommandValidator : Validator<UpdateCompanyRoleCommand>
    {
        public UpdateCompanyRoleCommandValidator()
        {
            Field(c => c.RoleId).Required();
            Field(c => c.Key).Required().Length(3, 100);
            Field(c => c.Name).Required().Length(3, 100);
        }
    }

    internal class UpdateCompanyRoleCommandComposer : Composer<UpdateCompanyRoleCommand>
    {
        public UpdateCompanyRoleCommandComposer(
            ITokenAccessor tokenAccessor,
            ICompanyRoleRepository companyRoleRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Role).LoadWith(ctx => companyRoleRepository.GetByIdAsync(ctx.Root.RoleId));
        }
    }
}