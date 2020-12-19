using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Users;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Users
{
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
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
            Field(c => c.Claim).LoadWith(ctx => claimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}