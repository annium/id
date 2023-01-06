using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Claims;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Claims;

internal class CreateClaimCommandValidator : Validator<CreateClaimCommand>
{
    public CreateClaimCommandValidator(
        IClaimRepository claimRepository
    )
    {
        Field(c => c.AppId).Required();
        Field(c => c.Key).Required().Length(3, 100).Then()
            .Unique(async (c, key) => await claimRepository.FindByKeyAsync(c.AppId, key) != null, "Claim with {1} {2} already exists");
        Field(c => c.Name).Required().Length(3, 100);
    }
}

internal class CreateClaimCommandComposer : Composer<CreateClaimCommand>
{
    public CreateClaimCommandComposer(
        ITokenAccessor tokenAccessor,
        IAppRepository appRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
    }
}