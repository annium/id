using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.CompanyRoles;

namespace Annium.Id.Api.ViewModels.Requests.CompanyRoles
{
    public class UpdateCompanyRoleRequest : UpdateCompanyRoleRequestBody, IRequest<UpdateCompanyRoleCommand>
    {
        public Guid RoleId { get; set; }
    }

    public class UpdateCompanyRoleRequestBody
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}