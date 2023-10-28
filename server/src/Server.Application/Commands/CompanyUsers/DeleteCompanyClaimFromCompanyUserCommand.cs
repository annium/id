using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.CompanyUsers;

namespace Server.Application.Commands.CompanyUsers;

internal class DeleteCompanyClaimFromCompanyUserCommandValidator : Validator<DeleteCompanyClaimFromCompanyUserCommand>
{
    public DeleteCompanyClaimFromCompanyUserCommandValidator()
    {
        Field(c => c.CompanyId).Required();
        Field(c => c.UserId).Required();
        Field(c => c.ClaimId).Required();
    }
}

internal class DeleteCompanyClaimFromCompanyUserCommandComposer : Composer<DeleteCompanyClaimFromCompanyUserCommand>
{
    public DeleteCompanyClaimFromCompanyUserCommandComposer(
        ITokenAccessor tokenAccessor,
        ICompanyRepository companyRepository,
        IUserRepository userRepository,
        ICompanyClaimRepository claimRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Company).LoadWith(ctx => companyRepository.TryGetByIdAsync(ctx.Root.CompanyId));
        Field(c => c.User).LoadWith(ctx => userRepository.TryGetByIdAsync(ctx.Root.UserId));
        Field(c => c.Claim).LoadWith(ctx => claimRepository.TryGetByIdAsync(ctx.Root.ClaimId));
    }
}
