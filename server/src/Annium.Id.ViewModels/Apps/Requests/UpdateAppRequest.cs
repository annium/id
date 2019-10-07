using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Apps;

namespace Annium.Id.ViewModels.Apps.Requests
{
    public class UpdateAppRequest : UpdateAppRequestBase, IRequest<UpdateAppCommand>
    {
        public Guid AppId { get; set; }
    }

    public class UpdateAppRequestBase
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}