using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.CompanyUsers;

namespace Server.ViewModels.Requests.CompanyUsers;

public class DeleteCompanyClaimFromCompanyUserRequest : IRequest<DeleteCompanyClaimFromCompanyUserCommand>
{
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
    public Guid ClaimId { get; set; }
}