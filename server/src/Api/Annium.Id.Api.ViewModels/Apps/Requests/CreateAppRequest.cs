using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Apps;

namespace Annium.Id.ViewModels.Apps.Requests
{
    public class CreateAppRequest : IRequest<CreateAppCommand>
    {
        public string Name { get; set; } = string.Empty;
    }
}