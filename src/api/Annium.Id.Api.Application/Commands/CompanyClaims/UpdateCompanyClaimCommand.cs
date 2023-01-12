using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.CompanyClaims;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.CompanyClaims;

internal class UpdateCompanyClaimCommandValidator : Validator<UpdateCompanyClaimCommand>
{
    public UpdateCompanyClaimCommandValidator()
    {
        Field(c => c.ClaimId).Required();
        Field(c => c.Key).Required().Length(3, 100);
        Field(c => c.Name).Required().Length(3, 100);
    }
}

internal class UpdateCompanyClaimCommandComposer : Composer<UpdateCompanyClaimCommand>
{
    public UpdateCompanyClaimCommandComposer(
        ITokenAccessor tokenAccessor,
        ICompanyClaimRepository companyClaimRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Claim).LoadWith(ctx => companyClaimRepository.TryGetByIdAsync(ctx.Root.ClaimId));
    }
}