using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Apps;

namespace Annium.Id.Api.ViewModels.Apps.Requests
{
    public class CreateAppRequest : IRequest<CreateAppCommand>
    {
        public string Name { get; set; } = string.Empty;
    }
}