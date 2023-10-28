using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.CompanyClaims;

namespace Server.Application.Commands.CompanyClaims;

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
