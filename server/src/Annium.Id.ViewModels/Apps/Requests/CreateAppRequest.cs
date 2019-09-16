using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Apps;

namespace Annium.Id.ViewModels.Apps.Requests
{
    public class CreateAppRequest : IRequest<CreateAppCommand>
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}