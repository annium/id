using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Claims;

namespace Server.Application.Commands.Claims;

internal class UpdateClaimCommandValidator : Validator<UpdateClaimCommand>
{
    public UpdateClaimCommandValidator()
    {
        Field(c => c.ClaimId).Required();
        Field(c => c.Key).Required().Length(3, 100);
        Field(c => c.Name).Required().Length(3, 100);
    }
}

internal class UpdateClaimCommandComposer : Composer<UpdateClaimCommand>
{
    public UpdateClaimCommandComposer(ITokenAccessor tokenAccessor, IClaimRepository claimRepository)
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Claim).LoadWith(ctx => claimRepository.TryGetByIdAsync(ctx.Root.ClaimId));
    }
}
