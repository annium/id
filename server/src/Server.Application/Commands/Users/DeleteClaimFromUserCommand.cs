using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Users;

namespace Server.Application.Commands.Users;

internal class DeleteClaimFromUserCommandValidator : Validator<DeleteClaimFromUserCommand>
{
    public DeleteClaimFromUserCommandValidator()
    {
        Field(c => c.UserId).Required();
        Field(c => c.ClaimId).Required();
    }
}

internal class DeleteClaimFromUserCommandComposer : Composer<DeleteClaimFromUserCommand>
{
    public DeleteClaimFromUserCommandComposer(
        ITokenAccessor tokenAccessor,
        IUserRepository userRepository,
        IClaimRepository claimRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.User).LoadWith(ctx => userRepository.TryGetByIdAsync(ctx.Root.UserId));
        Field(c => c.Claim).LoadWith(ctx => claimRepository.TryGetByIdAsync(ctx.Root.ClaimId));
    }
}