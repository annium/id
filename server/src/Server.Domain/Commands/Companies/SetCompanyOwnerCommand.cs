using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.Companies;

public class SetCompanyOwnerCommand : ICommand
{
    public Guid CompanyId { get; }
    public Guid UserId { get; }
    public Guid MyId { get; private set; }
    public Company Company { get; private set; } = null!;
    public User User { get; private set; } = null!;

    public SetCompanyOwnerCommand(
        Guid companyId,
        Guid userId
    )
    {
        CompanyId = companyId;
        UserId = userId;
    }
}