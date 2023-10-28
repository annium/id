using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Claims;

namespace Server.Application.Commands.Claims;

internal class CreateClaimCommandValidator : Validator<CreateClaimCommand>
{
    public CreateClaimCommandValidator(IClaimRepository claimRepository)
    {
        Field(c => c.AppId).Required();
        Field(c => c.Key)
            .Required()
            .Length(3, 100)
            .Then()
            .Unique(
                async (c, key) => await claimRepository.TryFindByKeyAsync(c.AppId, key) != null,
                "Claim with {1} {2} already exists"
            );
        Field(c => c.Name).Required().Length(3, 100);
    }
}

internal class CreateClaimCommandComposer : Composer<CreateClaimCommand>
{
    public CreateClaimCommandComposer(ITokenAccessor tokenAccessor, IAppRepository appRepository)
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}
