using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Users;

namespace Annium.Id.ViewModels.Users.Requests
{
    public class UpdateUserTokenRequest : IRequest<UpdateUserTokenCommand>
    {
        public Guid RefreshToken { get; set; }
    }
}