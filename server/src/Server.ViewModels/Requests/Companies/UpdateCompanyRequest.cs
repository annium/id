using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Companies;

namespace Server.ViewModels.Requests.Companies;

public class UpdateCompanyRequest : UpdateCompanyRequestBody, IRequest<UpdateCompanyCommand>
{
    public Guid CompanyId { get; set; }
}

public class UpdateCompanyRequestBody
{
    public Guid? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
}