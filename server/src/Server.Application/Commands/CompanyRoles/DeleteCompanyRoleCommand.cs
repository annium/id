using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.CompanyRoles;

namespace Server.Application.Commands.CompanyRoles;

internal class DeleteCompanyRoleCommandValidator : Validator<DeleteCompanyRoleCommand>
{
    public DeleteCompanyRoleCommandValidator()
    {
        Field(c => c.RoleId).Required();
    }
}

internal class DeleteCompanyRoleCommandComposer : Composer<DeleteCompanyRoleCommand>
{
    public DeleteCompanyRoleCommandComposer(ITokenAccessor tokenAccessor, ICompanyRoleRepository companyRoleRepository)
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Role).LoadWith(ctx => companyRoleRepository.TryGetByIdAsync(ctx.Root.RoleId));
    }
}
