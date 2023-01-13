using System;
using Annium.Architecture.ViewModel;
using Core.Domain.Entities;

namespace Server.ViewModels.Responses.Companies;

public class CompanyResponse : IResponse<Company>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}