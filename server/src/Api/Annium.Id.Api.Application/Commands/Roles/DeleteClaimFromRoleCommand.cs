using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Application.Commands.Roles
{
    public class DeleteClaimFromRoleCommand : ICommand
    {
        public Guid RoleId { get; }
        public Guid ClaimId { get; }
        public Guid MyId { get; private set; }
        public Role Role { get; private set; } = null!;
        public Claim Claim { get; private set; } = null!;

        public DeleteClaimFromRoleCommand(
            Guid roleId,
            Guid claimId
        )
        {
            RoleId = roleId;
            ClaimId = claimId;
        }
    }

    internal class DeleteClaimFromRoleCommandValidator : Validator<DeleteClaimFromRoleCommand>
    {
        public DeleteClaimFromRoleCommandValidator()
        {
            Field(c => c.RoleId).Required();
            Field(c => c.ClaimId).Required();
        }
    }

    internal class DeleteClaimFromRoleCommandComposer : Composer<DeleteClaimFromRoleCommand>
    {
        public DeleteClaimFromRoleCommandComposer(
            ITokenAccessor tokenAccessor,
            IRoleRepository roleRepository,
            IClaimRepository claimRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Role).LoadWith(ctx => roleRepository.GetByIdAsync(ctx.Root.RoleId));
            Field(c => c.Claim).LoadWith(ctx => claimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}