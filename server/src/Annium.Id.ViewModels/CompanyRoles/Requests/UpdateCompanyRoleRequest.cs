using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.CompanyRoles;

namespace Annium.Id.ViewModels.CompanyRoles.Requests
{
    public class UpdateCompanyRoleRequest : IRequest<UpdateCompanyRoleCommand>
    {
        public Guid AppId { get; set; }
        public Guid RoleId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}