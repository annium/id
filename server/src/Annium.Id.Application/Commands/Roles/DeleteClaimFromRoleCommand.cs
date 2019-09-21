using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Roles
{
    public class DeleteClaimFromRoleCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid RoleId { get; }
        public Guid ClaimId { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }
        public Role Role { get; private set; }
        public Claim Claim { get; private set; }

        public DeleteClaimFromRoleCommand(
            Guid appId,
            Guid roleId,
            Guid claimId
        )
        {
            AppId = appId;
            RoleId = roleId;
            ClaimId = claimId;
        }
    }

    internal class DeleteClaimFromRoleCommandValidator : Validator<DeleteClaimFromRoleCommand>
    {
        public DeleteClaimFromRoleCommandValidator()
        {
            Field(c => c.AppId).Required();
            Field(c => c.RoleId).Required();
            Field(c => c.ClaimId).Required();
        }
    }

    internal class DeleteClaimFromRoleCommandComposer : Composer<DeleteClaimFromRoleCommand>
    {
        public DeleteClaimFromRoleCommandComposer(
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