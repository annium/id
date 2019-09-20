using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.AppUsers;

namespace Annium.Id.ViewModels.AppUsers.Requests
{
    public class DeleteClaimFromUserRequest : IRequest<DeleteClaimFromUserCommand>
    {
        public Guid AppId { get; set; }
        public Guid UserId { get; set; }
        public Guid ClaimId { get; set; }
    }
}