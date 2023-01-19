using System;
using System.Collections.Generic;
using Annium.Architecture.ViewModel;
using Server.Domain.Models;
using Server.ViewModels.Responses.CompanyRoleClaims;

namespace Server.ViewModels.Responses.CompanyRoles;

public record CompanyRoleResponse : IResponse<CompanyRole>
{
    public Guid Id { get; set; }
    public Guid AppId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IReadOnlyCollection<CompanyRoleClaimResponse> Claims { get; set; } = Array.Empty<CompanyRoleClaimResponse>();
}