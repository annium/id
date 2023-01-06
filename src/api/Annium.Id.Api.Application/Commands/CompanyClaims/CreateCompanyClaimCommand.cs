using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.CompanyClaims;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.CompanyClaims;

internal class CreateCompanyClaimCommandValidator : Validator<CreateCompanyClaimCommand>
{
    public CreateCompanyClaimCommandValidator(
        ICompanyClaimRepository companyClaimRepository
    )
    {
        Field(c => c.AppId).Required();
        Field(c => c.Key).Required().Length(3, 100).Then()
            .Unique(async (c, key) => await companyClaimRepository.FindByKeyAsync(c.AppId, key) != null, "Company claim with {1} {2} already exists");
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
        Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
    }
}