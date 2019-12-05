using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Me;

namespace Annium.Id.ViewModels.Me.Requests
{
    public class RegisterMeRequest : IRequest<RegisterMeCommand>
    {
        public string Server { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
    }
}