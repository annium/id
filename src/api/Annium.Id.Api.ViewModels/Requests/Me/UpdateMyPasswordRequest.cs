using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Me;

namespace Annium.Id.Api.ViewModels.Requests.Me;

public class UpdateMyPasswordRequest : IRequest<UpdateMyPasswordCommand>
{
    public string Password { get; set; } = string.Empty;
}