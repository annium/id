using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Me;

namespace Annium.Id.ViewModels.Me.Requests
{
    public class UpdateMyPasswordRequest : IRequest<UpdateMyPasswordCommand>
    {
        public string Password { get; set; } = string.Empty;
    }
}