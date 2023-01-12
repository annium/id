using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.CompanyRoles;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.CompanyRoles;

internal class UpdateCompanyRoleCommandValidator : Validator<UpdateCompanyRoleCommand>
{
    public UpdateCompanyRoleCommandValidator()
    {
        Field(c => c.RoleId).Required();
        Field(c => c.Key).Required().Length(3, 100);
        Field(c => c.Name).Required().Length(3, 100);
    }
}

internal class UpdateCompanyRoleCommandComposer : Composer<UpdateCompanyRoleCommand>
{
    public UpdateCompanyRoleCommandComposer(
        ITokenAccessor tokenAccessor,
        ICompanyRoleRepository companyRoleRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Role).LoadWith(ctx => companyRoleRepository.TryGetByIdAsync(ctx.Root.RoleId));
    }
}