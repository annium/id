using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Roles;

namespace Annium.Id.ViewModels.Roles.Requests
{
    public class CreateRoleRequest : IRequest<CreateRoleCommand>
    {
        public Guid AppId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}