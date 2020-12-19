using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.CompanyRoles
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
}