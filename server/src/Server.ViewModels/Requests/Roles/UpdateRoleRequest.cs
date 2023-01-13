using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Roles;

namespace Server.ViewModels.Requests.Roles;

public class UpdateRoleRequest : UpdateRoleRequestBody, IRequest<UpdateRoleCommand>
{
    public Guid RoleId { get; set; }
}

public class UpdateRoleRequestBody
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}