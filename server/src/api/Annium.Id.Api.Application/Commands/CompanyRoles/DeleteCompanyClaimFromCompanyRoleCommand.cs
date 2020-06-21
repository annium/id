using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.CompanyRoles
{
    public class DeleteCompanyClaimFromCompanyRoleCommand : ICommand
    {
        public Guid RoleId { get; }
        public Guid ClaimId { get; }
        public Guid MyId { get; private set; }
        public CompanyRole Role { get; private set; } = null!;
        public CompanyClaim Claim { get; private set; } = null!;

        public DeleteCompanyClaimFromCompanyRoleCommand(
            Guid roleId,
            Guid claimId
        )
        {
            RoleId = roleId;
            ClaimId = claimId;
        }
    }

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
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Role).LoadWith(ctx => companyRoleRepository.GetByIdAsync(ctx.Root.RoleId));
            Field(c => c.Claim).LoadWith(ctx => companyClaimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}