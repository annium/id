using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Users;

namespace Annium.Id.Api.ViewModels.Requests.Users
{
    public class AddClaimToUserRequest : AddClaimToUserRequestBody, IRequest<AddClaimToUserCommand>
    {
        public Guid UserId { get; set; }
        public Guid ClaimId { get; set; }
    }

    public class AddClaimToUserRequestBody
    {
        public string Value { get; set; } = string.Empty;
    }
}