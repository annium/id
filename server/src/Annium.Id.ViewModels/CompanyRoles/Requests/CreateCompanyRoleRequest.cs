using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.CompanyRoles;

namespace Annium.Id.ViewModels.CompanyRoles.Requests
{
    public class CreateCompanyRoleRequest : IRequest<CreateCompanyRoleCommand>
    {
        public Guid AppId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}