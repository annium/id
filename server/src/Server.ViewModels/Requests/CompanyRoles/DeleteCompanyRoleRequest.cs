using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.CompanyRoles;

namespace Server.ViewModels.Requests.CompanyRoles;

public class DeleteCompanyRoleRequest : IRequest<DeleteCompanyRoleCommand>
{
    public Guid RoleId { get; set; }
}