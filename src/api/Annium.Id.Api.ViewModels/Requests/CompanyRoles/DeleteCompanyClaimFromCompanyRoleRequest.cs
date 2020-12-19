using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.CompanyRoles;

namespace Annium.Id.Api.ViewModels.Requests.CompanyRoles
{
    public class DeleteCompanyClaimFromCompanyRoleRequest : IRequest<DeleteCompanyClaimFromCompanyRoleCommand>
    {
        public Guid RoleId { get; set; }
        public Guid ClaimId { get; set; }
    }
}