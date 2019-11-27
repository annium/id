using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Me;

namespace Annium.Id.ViewModels.Me.Requests
{
    public class RestoreMyAccessRequest : RestoreMyAccessRequestBase, IRequest<RestoreMyAccessCommand>
    {
        public string AppKey { get; set; } = string.Empty;
    }

    public class RestoreMyAccessRequestBase
    {
        public string Email { get; set; } = string.Empty;
    }
}