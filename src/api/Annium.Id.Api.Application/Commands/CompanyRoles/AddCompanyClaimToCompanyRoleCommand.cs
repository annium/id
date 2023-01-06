using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.CompanyRoles;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.CompanyRoles;

internal class AddCompanyClaimToCompanyRoleCommandValidator : Validator<AddCompanyClaimToCompanyRoleCommand>
{
    public AddCompanyClaimToCompanyRoleCommandValidator()
    {
        Field(c => c.RoleId).Required();
        Field(c => c.ClaimId).Required();
        Field(c => c.Value).Required().Length(3, 100);
    }
}

internal class AddCompanyClaimToCompanyRoleCommandComposer : Composer<AddCompanyClaimToCompanyRoleCommand>
{
    public AddCompanyClaimToCompanyRoleCommandComposer(
        ITokenAccessor tokenAccessor,
        ICompanyRoleRepository companyRoleRepository,
        ICompanyClaimRepository companyClaimRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Role).LoadWith(ctx => companyRoleRepository.GetByIdAsync(ctx.Root.RoleId));
        Field(c => c.Claim).LoadWith(ctx => companyClaimRepository.GetByIdAsync(ctx.Root.ClaimId));
    }
}