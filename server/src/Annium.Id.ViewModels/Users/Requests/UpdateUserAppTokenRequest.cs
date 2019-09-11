using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Users;

namespace Annium.Id.ViewModels.Users.Requests
{
    public class UpdateUserAppTokenRequest : IRequest<UpdateUserAppTokenCommand>
    {
        public Guid AppId { get; set; }
        public Guid RefreshToken { get; set; }
    }
}