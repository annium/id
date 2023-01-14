using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Models;
using Server.ViewModels.Responses.CompanyClaims;

namespace Server.ViewModels.Responses.CompanyRoles;

public class CompanyRoleResponse : IResponse<CompanyRole>
{
    public Guid Id { get; set; }
    public Guid AppId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public CompanyClaimValueResponse[] Claims { get; set; } = Array.Empty<CompanyClaimValueResponse>();
}