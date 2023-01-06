using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Apps;

namespace Annium.Id.Api.ViewModels.Requests.Apps;

public class CreateAppRequest : IRequest<CreateAppCommand>
{
    public string Name { get; set; } = string.Empty;
}