using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Me;

namespace Annium.Id.Api.ViewModels.Me.Requests
{
    public class UpdateMyPasswordRequest : IRequest<UpdateMyPasswordCommand>
    {
        public string Password { get; set; } = string.Empty;
    }
}