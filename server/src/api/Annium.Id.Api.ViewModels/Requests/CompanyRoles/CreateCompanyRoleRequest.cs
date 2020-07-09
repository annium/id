using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.CompanyRoles;

namespace Annium.Id.Api.ViewModels.Requests.CompanyRoles
{
    public class CreateCompanyRoleRequest : IRequest<CreateCompanyRoleCommand>
    {
        public Guid AppId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}