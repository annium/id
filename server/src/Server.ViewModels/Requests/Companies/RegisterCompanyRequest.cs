using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Companies;

namespace Server.ViewModels.Requests.Companies;

public class RegisterCompanyRequest : IRequest<RegisterCompanyCommand>
{
    public Guid? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
}