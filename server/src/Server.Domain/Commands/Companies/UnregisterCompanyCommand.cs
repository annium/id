using System;
using Annium.Architecture.CQRS.Commands;
using Core.Domain.Entities;

namespace Server.Domain.Commands.Companies;

public class UnregisterCompanyCommand : ICommand
{
    public Guid CompanyId { get; }
    public Guid MyId { get; private set; }
    public Company Company { get; private set; } = null!;

    public UnregisterCompanyCommand(
        Guid companyId
    )
    {
        CompanyId = companyId;
    }
}