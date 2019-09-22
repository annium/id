using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Me;

namespace Annium.Id.ViewModels.Me.Requests
{
    public class RegisterMeRequest : IRequest<RegisterMeCommand>
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}