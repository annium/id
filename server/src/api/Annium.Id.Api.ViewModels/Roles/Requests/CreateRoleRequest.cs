using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Roles;

namespace Annium.Id.Api.ViewModels.Roles.Requests
{
    public class CreateRoleRequest : IRequest<CreateRoleCommand>
    {
        public Guid AppId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}