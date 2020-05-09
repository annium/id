using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.CompanyRoles;

namespace Annium.Id.ViewModels.CompanyRoles.Requests
{
    public class DeleteCompanyClaimFromCompanyRoleRequest : IRequest<DeleteCompanyClaimFromCompanyRoleCommand>
    {
        public Guid RoleId { get; set; }
        public Guid ClaimId { get; set; }
    }
}