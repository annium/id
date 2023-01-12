using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Claims;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Claims;

internal class DeleteClaimCommandValidator : Validator<DeleteClaimCommand>
{
    public DeleteClaimCommandValidator()
    {
        Field(c => c.ClaimId).Required();
    }
}

internal class DeleteClaimCommandComposer : Composer<DeleteClaimCommand>
{
    public DeleteClaimCommandComposer(
        ITokenAccessor tokenAccessor,
        IClaimRepository claimRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Claim).LoadWith(ctx => claimRepository.TryGetByIdAsync(ctx.Root.ClaimId));
    }
}