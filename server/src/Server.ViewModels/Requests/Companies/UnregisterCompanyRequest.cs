using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Companies;

namespace Server.ViewModels.Requests.Companies;

public class UnregisterCompanyRequest : IRequest<UnregisterCompanyCommand>
{
    public Guid CompanyId { get; set; }
}