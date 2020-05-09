using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Users;

namespace Annium.Id.ViewModels.Users.Requests
{
    public class AddClaimToUserRequest : AddClaimToUserRequestBase, IRequest<AddClaimToUserCommand>
    {
        public Guid UserId { get; set; }
        public Guid ClaimId { get; set; }
    }

    public class AddClaimToUserRequestBase
    {
        public string Value { get; set; } = string.Empty;
    }
}