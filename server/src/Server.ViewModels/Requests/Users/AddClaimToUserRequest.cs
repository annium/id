using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Users;

namespace Server.ViewModels.Requests.Users;

public record AddClaimToUserRequest : AddClaimToUserRequestBody, IRequest<AddClaimToUserCommand>
{
    public Guid UserId { get; set; }
    public Guid ClaimId { get; set; }
}

public record AddClaimToUserRequestBody
{
    public string Value { get; set; } = string.Empty;
}