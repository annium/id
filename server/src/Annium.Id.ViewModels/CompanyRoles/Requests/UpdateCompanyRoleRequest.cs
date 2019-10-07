using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.CompanyRoles;

namespace Annium.Id.ViewModels.CompanyRoles.Requests
{
    public class UpdateCompanyRoleRequest : UpdateCompanyRoleRequestBase, IRequest<UpdateCompanyRoleCommand>
    {
        public Guid RoleId { get; set; }
    }

    public class UpdateCompanyRoleRequestBase
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}