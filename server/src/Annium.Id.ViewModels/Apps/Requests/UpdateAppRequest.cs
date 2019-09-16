using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Apps;

namespace Annium.Id.ViewModels.Apps.Requests
{
    public class UpdateAppRequest : IRequest<UpdateAppCommand>
    {
        public Guid AppId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}