using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Roles;

namespace Annium.Id.Api.ViewModels.Requests.Roles
{
    public class DeleteClaimFromRoleRequest : IRequest<DeleteClaimFromRoleCommand>
    {
        public Guid RoleId { get; set; }
        public Guid ClaimId { get; set; }
    }
}