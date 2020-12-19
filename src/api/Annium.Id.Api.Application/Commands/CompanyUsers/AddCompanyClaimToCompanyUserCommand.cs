using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.CompanyUsers;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.CompanyUsers
{
    internal class AddCompanyClaimToCompanyUserCommandValidator : Validator<AddCompanyClaimToCompanyUserCommand>
    {
        public AddCompanyClaimToCompanyUserCommandValidator()
        {
            Field(c => c.CompanyId).Required();
            Field(c => c.UserId).Required();
            Field(c => c.ClaimId).Required();
            Field(c => c.Value).Required().Length(3, 100);
        }
    }

    internal class AddCompanyClaimToCompanyUserCommandComposer : Composer<AddCompanyClaimToCompanyUserCommand>
    {
        public AddCompanyClaimToCompanyUserCommandComposer(
            ITokenAccessor tokenAccessor,
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            ICompanyClaimRepository claimRepository
        )
        {
            Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
            Field(c => c.Company).LoadWith(ctx => companyRepository.GetByIdAsync(ctx.Root.CompanyId));
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
            Field(c => c.Claim).LoadWith(ctx => claimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}