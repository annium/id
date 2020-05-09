using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Users;

namespace Annium.Id.ViewModels.Users.Requests
{
    public class DeleteClaimFromUserRequest : IRequest<DeleteClaimFromUserCommand>
    {
        public Guid UserId { get; set; }
        public Guid ClaimId { get; set; }
    }
}