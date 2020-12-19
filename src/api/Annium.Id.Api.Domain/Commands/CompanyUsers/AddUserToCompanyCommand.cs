using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.CompanyUsers
{
    public class AddUserToCompanyCommand : ICommand
    {
        public Guid CompanyId { get; }
        public Guid UserId { get; }
        public Guid MyId { get; private set; }
        public Company Company { get; private set; } = null!;
        public User User { get; private set; } = null!;

        public AddUserToCompanyCommand(
            Guid companyId,
            Guid userId
        )
        {
            CompanyId = companyId;
            UserId = userId;
        }
    }
}