using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.CompanyRoles;

namespace Annium.Id.ViewModels.CompanyRoles.Requests
{
    public class AddCompanyClaimToCompanyRoleRequest : IRequest<AddCompanyClaimToCompanyRoleCommand>
    {
        public Guid RoleId { get; set; }
        public Guid ClaimId { get; set; }
        public string Value { get; set; }
    }
}