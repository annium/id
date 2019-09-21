using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Roles
{
    public class AddClaimToRoleCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid RoleId { get; }
        public Guid ClaimId { get; }
        public string Value { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }
        public Role Role { get; private set; }
        public Claim Claim { get; private set; }

        public AddClaimToRoleCommand(
            Guid appId,
            Guid roleId,
            Guid claimId,
            string value
        )
        {
            AppId = appId;
            RoleId = roleId;
            ClaimId = claimId;
            Value = value;
        }
    }

    internal class AddClaimToRoleCommandValidator : Validator<AddClaimToRoleCommand>
    {
        public AddClaimToRoleCommandValidator()
        {
            Field(c => c.AppId).Required();
            Field(c => c.RoleId).Required();
            Field(c => c.ClaimId).Required();
            Field(c => c.Value).Required().Length(3, 100);
        }
    }

    internal class AddClaimToRoleCommandComposer : Composer<AddClaimToRoleCommand>
    {
        public AddClaimToRoleCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository,
            IRoleRepository roleRepository,
            IClaimRepository claimRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.Role).LoadWith(ctx => roleRepository.GetByIdAsync(ctx.Root.RoleId));
            Field(c => c.Claim).LoadWith(ctx => claimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}