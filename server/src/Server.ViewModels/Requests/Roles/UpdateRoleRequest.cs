using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Roles;

namespace Server.ViewModels.Requests.Roles;

public record UpdateRoleRequest : UpdateRoleRequestBody, IRequest<UpdateRoleCommand>
{
    public Guid RoleId { get; set; }
}

public record UpdateRoleRequestBody
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}