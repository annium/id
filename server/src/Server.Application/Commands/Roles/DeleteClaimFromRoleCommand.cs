using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Roles;

namespace Server.Application.Commands.Roles;

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
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Role).LoadWith(ctx => roleRepository.TryGetByIdAsync(ctx.Root.RoleId));
        Field(c => c.Claim).LoadWith(ctx => claimRepository.TryGetByIdAsync(ctx.Root.ClaimId));
    }
}
