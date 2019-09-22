using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Me;

namespace Annium.Id.ViewModels.Me.Requests
{
    public class UpdateMeRequest : IRequest<UpdateMeCommand>
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}