using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.AppUsers;

namespace Annium.Id.ViewModels.AppUsers.Requests
{
    public class AddClaimToUserRequest : IRequest<AddClaimToUserCommand>
    {
        public Guid AppId { get; set; }
        public Guid UserId { get; set; }
        public Guid ClaimId { get; set; }
        public string Value { get; set; }
    }
}