using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.CompanyRoles;

namespace Annium.Id.Api.ViewModels.Requests.CompanyRoles
{
    public class DeleteCompanyRoleRequest : IRequest<DeleteCompanyRoleCommand>
    {
        public Guid RoleId { get; set; }
    }
}