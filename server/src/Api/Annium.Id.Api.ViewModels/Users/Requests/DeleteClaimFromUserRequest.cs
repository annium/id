using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Users;

namespace Annium.Id.Api.ViewModels.Users.Requests
{
    public class DeleteClaimFromUserRequest : IRequest<DeleteClaimFromUserCommand>
    {
        public Guid UserId { get; set; }
        public Guid ClaimId { get; set; }
    }
}