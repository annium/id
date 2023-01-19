using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.CompanyRoles;

namespace Server.ViewModels.Requests.CompanyRoles;

public record DeleteCompanyRoleRequest : IRequest<DeleteCompanyRoleCommand>
{
    public Guid RoleId { get; set; }
}