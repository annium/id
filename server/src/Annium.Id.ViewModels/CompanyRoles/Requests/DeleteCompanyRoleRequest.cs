using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.CompanyRoles;

namespace Annium.Id.ViewModels.CompanyRoles.Requests
{
    public class DeleteCompanyRoleRequest : IRequest<DeleteCompanyRoleCommand>
    {
        public Guid RoleId { get; set; }
    }
}