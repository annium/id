using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Me;

namespace Server.ViewModels.Requests.Me;

public class UpdateMyPasswordRequest : IRequest<UpdateMyPasswordCommand>
{
    public string Password { get; set; } = string.Empty;
}