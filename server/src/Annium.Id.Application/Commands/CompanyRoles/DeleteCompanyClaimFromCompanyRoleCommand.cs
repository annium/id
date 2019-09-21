using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.CompanyRoles
{
    public class DeleteCompanyClaimFromCompanyRoleCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid RoleId { get; }
        public Guid ClaimId { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }
        public CompanyRole Role { get; private set; }
        public CompanyClaim Claim { get; private set; }

        public DeleteCompanyClaimFromCompanyRoleCommand(
            Guid appId,
            Guid roleId,
            Guid claimId
        )
        {
            AppId = appId;
            RoleId = roleId;
            ClaimId = claimId;
        }
    }

    internal class DeleteCompanyClaimFromCompanyRoleCommandValidator : Validator<DeleteCompanyClaimFromCompanyRoleCommand>
    {
        public DeleteCompanyClaimFromCompanyRoleCommandValidator()
        {
            Field(c => c.AppId).Required();
            Field(c => c.RoleId).Required();
            Field(c => c.ClaimId).Required();
        }
    }

    internal class DeleteCompanyClaimFromCompanyRoleCommandComposer : Composer<DeleteCompanyClaimFromCompanyRoleCommand>
    {
        public DeleteCompanyClaimFromCompanyRoleCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository,
            ICompanyRoleRepository companyRoleRepository,
            ICompanyClaimRepository companyClaimRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.Role).LoadWith(ctx => companyRoleRepository.GetByIdAsync(ctx.Root.RoleId));
            Field(c => c.Claim).LoadWith(ctx => companyClaimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}