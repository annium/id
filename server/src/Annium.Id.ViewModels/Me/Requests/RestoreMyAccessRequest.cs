using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Me;

namespace Annium.Id.ViewModels.Me.Requests
{
    public class RestoreMyAccessRequest : RestoreMyAccessRequestBase, IRequest<RestoreMyAccessCommand>
    {
        public Guid AppId { get; set; }
    }

    public class RestoreMyAccessRequestBase
    {
        public string Server { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}