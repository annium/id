using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.CompanyRoles;

namespace Annium.Id.Api.ViewModels.CompanyRoles.Requests
{
    public class DeleteCompanyRoleRequest : IRequest<DeleteCompanyRoleCommand>
    {
        public Guid RoleId { get; set; }
    }
}