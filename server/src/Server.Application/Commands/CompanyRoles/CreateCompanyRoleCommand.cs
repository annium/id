using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.CompanyRoles;

namespace Server.Application.Commands.CompanyRoles;

internal class CreateCompanyRoleCommandValidator : Validator<CreateCompanyRoleCommand>
{
    public CreateCompanyRoleCommandValidator(ICompanyRoleRepository companyRoleRepository)
    {
        Field(c => c.AppId).Required();
        Field(c => c.Key)
            .Required()
            .Length(3, 100)
            .Then()
            .Unique(
                async (c, key) => await companyRoleRepository.TryFindByKeyAsync(c.AppId, key) != null,
                "Company role with {1} {2} already exists"
            );
        Field(c => c.Name).Required().Length(3, 100);
    }
}

internal class CreateCompanyRoleCommandComposer : Composer<CreateCompanyRoleCommand>
{
    public CreateCompanyRoleCommandComposer(ITokenAccessor tokenAccessor, IAppRepository appRepository)
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}
