using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.CompanyRoles;

namespace Server.ViewModels.Requests.CompanyRoles;

public record DeleteCompanyClaimFromCompanyRoleRequest : IRequest<DeleteCompanyClaimFromCompanyRoleCommand>
{
    public Guid RoleId { get; set; }
    public Guid ClaimId { get; set; }
}