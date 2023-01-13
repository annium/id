using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.CompanyUsers;

namespace Server.ViewModels.Requests.CompanyUsers;

public class AddCompanyClaimToCompanyUserRequest : AddCompanyClaimToCompanyUserRequestBody, IRequest<AddCompanyClaimToCompanyUserCommand>
{
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
    public Guid ClaimId { get; set; }
}

public class AddCompanyClaimToCompanyUserRequestBody
{
    public string Value { get; set; } = string.Empty;
}