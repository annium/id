using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.CompanyRoles
{
    public class AddCompanyClaimToCompanyRoleCommand : ICommand
    {
        public Guid RoleId { get; }
        public Guid ClaimId { get; }
        public string Value { get; }
        public Guid MyId { get; private set; }
        public CompanyRole Role { get; private set; }
        public CompanyClaim Claim { get; private set; }

        public AddCompanyClaimToCompanyRoleCommand(
            Guid roleId,
            Guid claimId,
            string value
        )
        {
            RoleId = roleId;
            ClaimId = claimId;
            Value = value;
        }
    }

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
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Role).LoadWith(ctx => companyRoleRepository.GetByIdAsync(ctx.Root.RoleId));
            Field(c => c.Claim).LoadWith(ctx => companyClaimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}