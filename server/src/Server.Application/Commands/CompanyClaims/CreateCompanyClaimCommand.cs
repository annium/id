using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.CompanyClaims;

namespace Server.Application.Commands.CompanyClaims;

internal class CreateCompanyClaimCommandValidator : Validator<CreateCompanyClaimCommand>
{
    public CreateCompanyClaimCommandValidator(
        ICompanyClaimRepository companyClaimRepository
    )
    {
        Field(c => c.AppId).Required();
        Field(c => c.Key).Required().Length(3, 100).Then()
            .Unique(async (c, key) => await companyClaimRepository.TryFindByKeyAsync(c.AppId, key) != null, "Company claim with {1} {2} already exists");
        Field(c => c.Name).Required().Length(3, 100);
    }
}

internal class CreateCompanyClaimCommandComposer : Composer<CreateCompanyClaimCommand>
{
    public CreateCompanyClaimCommandComposer(
        ITokenAccessor tokenAccessor,
        IAppRepository appRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}