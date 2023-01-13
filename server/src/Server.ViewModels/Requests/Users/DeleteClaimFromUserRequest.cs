using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Users;

namespace Server.ViewModels.Requests.Users;

public class DeleteClaimFromUserRequest : IRequest<DeleteClaimFromUserCommand>
{
    public Guid UserId { get; set; }
    public Guid ClaimId { get; set; }
}