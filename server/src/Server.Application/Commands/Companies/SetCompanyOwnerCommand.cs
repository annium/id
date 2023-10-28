using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Companies;

namespace Server.Application.Commands.Companies;

internal class SetCompanyOwnerCommandValidator : Validator<SetCompanyOwnerCommand>
{
    public SetCompanyOwnerCommandValidator()
    {
        Field(c => c.CompanyId).Required();
        Field(c => c.UserId).Required();
    }
}

internal class SetCompanyOwnerCommandComposer : Composer<SetCompanyOwnerCommand>
{
    public SetCompanyOwnerCommandComposer(
        ITokenAccessor tokenAccessor,
        ICompanyRepository companyRepository,
        IUserRepository userRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Company).LoadWith(ctx => companyRepository.TryGetByIdAsync(ctx.Root.CompanyId));
        Field(c => c.User).LoadWith(ctx => userRepository.TryGetByIdAsync(ctx.Root.UserId));
    }
}
