using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.CompanyRoles;

namespace Annium.Id.Api.ViewModels.Requests.CompanyRoles;

public class AddCompanyClaimToCompanyRoleRequest : AddCompanyClaimToCompanyRoleRequestBody, IRequest<AddCompanyClaimToCompanyRoleCommand>
{
    public Guid RoleId { get; set; }
    public Guid ClaimId { get; set; }
}

public class AddCompanyClaimToCompanyRoleRequestBody
{
    public string Value { get; set; } = string.Empty;
}