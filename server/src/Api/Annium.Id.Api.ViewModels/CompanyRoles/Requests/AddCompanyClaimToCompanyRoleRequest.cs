using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.CompanyRoles;

namespace Annium.Id.Api.ViewModels.CompanyRoles.Requests
{
    public class AddCompanyClaimToCompanyRoleRequest : AddCompanyClaimToCompanyRoleRequestBase, IRequest<AddCompanyClaimToCompanyRoleCommand>
    {
        public Guid RoleId { get; set; }
        public Guid ClaimId { get; set; }
    }

    public class AddCompanyClaimToCompanyRoleRequestBase
    {
        public string Value { get; set; } = string.Empty;
    }
}