using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Me;

namespace Server.ViewModels.Requests.Me;

public record UpdateMyProfileRequest : IRequest<UpdateMyProfileCommand>
{
    public string Login { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}