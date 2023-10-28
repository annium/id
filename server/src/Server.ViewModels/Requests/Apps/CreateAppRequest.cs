using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Apps;

namespace Server.ViewModels.Requests.Apps;

public record CreateAppRequest : IRequest<CreateAppCommand>
{
    public string Name { get; set; } = string.Empty;
}
