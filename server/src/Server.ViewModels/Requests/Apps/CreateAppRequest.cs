using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Apps;

namespace Server.ViewModels.Requests.Apps;

public class CreateAppRequest : IRequest<CreateAppCommand>
{
    public string Name { get; set; } = string.Empty;
}