using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.CompanyRoles;

namespace Server.ViewModels.Requests.CompanyRoles;

public record UpdateCompanyRoleRequest : UpdateCompanyRoleRequestBody, IRequest<UpdateCompanyRoleCommand>
{
    public Guid RoleId { get; set; }
}

public record UpdateCompanyRoleRequestBody
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}