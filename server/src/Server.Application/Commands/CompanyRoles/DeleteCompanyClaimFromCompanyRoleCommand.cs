using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.CompanyRoles;

namespace Server.Application.Commands.CompanyRoles;

internal class DeleteCompanyClaimFromCompanyRoleCommandValidator : Validator<DeleteCompanyClaimFromCompanyRoleCommand>
{
    public DeleteCompanyClaimFromCompanyRoleCommandValidator()
    {
        Field(c => c.RoleId).Required();
        Field(c => c.ClaimId).Required();
    }
}

internal class DeleteCompanyClaimFromCompanyRoleCommandComposer : Composer<DeleteCompanyClaimFromCompanyRoleCommand>
{
    public DeleteCompanyClaimFromCompanyRoleCommandComposer(
        ITokenAccessor tokenAccessor,
        ICompanyRoleRepository companyRoleRepository,
        ICompanyClaimRepository companyClaimRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Role).LoadWith(ctx => companyRoleRepository.TryGetByIdAsync(ctx.Root.RoleId));
        Field(c => c.Claim).LoadWith(ctx => companyClaimRepository.TryGetByIdAsync(ctx.Root.ClaimId));
    }
}
