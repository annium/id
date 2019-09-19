using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Roles;

namespace Annium.Id.ViewModels.Roles.Requests
{
    public class DeleteClaimFromRoleRequest : IRequest<DeleteClaimFromRoleCommand>
    {
        public Guid AppId { get; set; }
        public Guid RoleId { get; set; }
        public Guid ClaimId { get; set; }
    }
}