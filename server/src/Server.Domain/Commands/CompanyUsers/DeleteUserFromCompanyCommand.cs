using System;
using Annium.Architecture.CQRS.Commands;
using Core.Domain.Entities;

namespace Server.Domain.Commands.CompanyUsers;

public class DeleteUserFromCompanyCommand : ICommand
{
    public Guid CompanyId { get; }
    public Guid UserId { get; }
    public Guid MyId { get; private set; }
    public Company Company { get; private set; } = null!;
    public User User { get; private set; } = null!;

    public DeleteUserFromCompanyCommand(
        Guid companyId,
        Guid userId
    )
    {
        CompanyId = companyId;
        UserId = userId;
    }
}