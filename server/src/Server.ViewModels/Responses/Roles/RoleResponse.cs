using System;
using System.Collections.Generic;
using Annium.Architecture.ViewModel;
using Server.Domain.Models;
using Server.ViewModels.Responses.RoleClaims;

namespace Server.ViewModels.Responses.Roles;

public record RoleResponse : IResponse<Role>
{
    public Guid Id { get; set; }
    public Guid AppId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IReadOnlyCollection<RoleClaimResponse> Claims { get; set; } = Array.Empty<RoleClaimResponse>();
}