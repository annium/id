using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Me;

namespace Annium.Id.Api.ViewModels.Requests.Me
{
    public class UpdateMyProfileRequest : IRequest<UpdateMyProfileCommand>
    {
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}