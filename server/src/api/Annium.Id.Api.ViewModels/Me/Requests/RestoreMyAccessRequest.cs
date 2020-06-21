using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Me;

namespace Annium.Id.Api.ViewModels.Me.Requests
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