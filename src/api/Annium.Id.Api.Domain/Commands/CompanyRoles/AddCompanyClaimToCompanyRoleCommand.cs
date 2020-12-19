using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.CompanyRoles
{
    public class AddCompanyClaimToCompanyRoleCommand : ICommand
    {
        public Guid RoleId { get; }
        public Guid ClaimId { get; }
        public string Value { get; }
        public Guid MyId { get; private set; }
        public CompanyRole Role { get; private set; } = null!;
        public CompanyClaim Claim { get; private set; } = null!;

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
}