using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Roles;

namespace Annium.Id.ViewModels.Roles.Requests
{
    public class AddClaimToRoleRequest : AddClaimToRoleRequestBase, IRequest<AddClaimToRoleCommand>
    {
        public Guid RoleId { get; set; }
        public Guid ClaimId { get; set; }
    }

    public class AddClaimToRoleRequestBase
    {
        public string Value { get; set; } = string.Empty;
    }
}