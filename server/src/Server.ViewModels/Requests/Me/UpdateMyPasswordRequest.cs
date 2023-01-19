using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Me;

namespace Server.ViewModels.Requests.Me;

public record UpdateMyPasswordRequest : IRequest<UpdateMyPasswordCommand>
{
    public string Password { get; set; } = string.Empty;
}