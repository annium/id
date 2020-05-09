using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Roles;

namespace Annium.Id.Api.ViewModels.Roles.Requests
{
    public class DeleteClaimFromRoleRequest : IRequest<DeleteClaimFromRoleCommand>
    {
        public Guid RoleId { get; set; }
        public Guid ClaimId { get; set; }
    }
}