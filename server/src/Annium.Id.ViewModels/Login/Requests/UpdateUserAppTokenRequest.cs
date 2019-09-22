using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Login;

namespace Annium.Id.ViewModels.Login.Requests
{
    public class UpdateUserAppTokenRequest : IRequest<UpdateUserAppTokenCommand>
    {
        public Guid AppId { get; set; }
        public Guid RefreshToken { get; set; }
    }
}