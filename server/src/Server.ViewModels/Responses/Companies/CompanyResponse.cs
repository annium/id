using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Models;

namespace Server.ViewModels.Responses.Companies;

public record CompanyResponse : IResponse<Company>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
