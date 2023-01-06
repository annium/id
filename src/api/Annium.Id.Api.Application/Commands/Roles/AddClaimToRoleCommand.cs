using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Roles;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Roles;

internal class AddClaimToRoleCommandValidator : Validator<AddClaimToRoleCommand>
{
    public AddClaimToRoleCommandValidator()
    {
        Field(c => c.RoleId).Required();
        Field(c => c.ClaimId).Required();
        Field(c => c.Value).Required().Length(3, 100);
    }
}

internal class AddClaimToRoleCommandComposer : Composer<AddClaimToRoleCommand>
{
    public AddClaimToRoleCommandComposer(
        ITokenAccessor tokenAccessor,
        IRoleRepository roleRepository,
        IClaimRepository claimRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Role).LoadWith(ctx => roleRepository.GetByIdAsync(ctx.Root.RoleId));
        Field(c => c.Claim).LoadWith(ctx => claimRepository.GetByIdAsync(ctx.Root.ClaimId));
    }
}