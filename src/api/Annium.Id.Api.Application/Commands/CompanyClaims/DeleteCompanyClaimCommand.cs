using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.CompanyClaims;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.CompanyClaims;

internal class DeleteCompanyClaimCommandValidator : Validator<DeleteCompanyClaimCommand>
{
    public DeleteCompanyClaimCommandValidator()
    {
        Field(c => c.ClaimId).Required();
    }
}

internal class DeleteCompanyClaimCommandComposer : Composer<DeleteCompanyClaimCommand>
{
    public DeleteCompanyClaimCommandComposer(
        ITokenAccessor tokenAccessor,
        ICompanyClaimRepository companyClaimRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Claim).LoadWith(ctx => companyClaimRepository.TryGetByIdAsync(ctx.Root.ClaimId));
    }
}