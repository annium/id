using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.CompanyRoles;

namespace Server.ViewModels.Requests.CompanyRoles;

public record CreateCompanyRoleRequest : IRequest<CreateCompanyRoleCommand>
{
    public Guid AppId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}