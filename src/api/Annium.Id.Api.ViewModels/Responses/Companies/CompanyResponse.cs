using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.ViewModels.Responses.Companies;

public class CompanyResponse : IResponse<Company>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}