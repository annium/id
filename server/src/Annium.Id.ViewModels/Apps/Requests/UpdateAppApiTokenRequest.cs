using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Apps;

namespace Annium.Id.ViewModels.Apps.Requests
{
    public class UpdateAppApiTokenRequest : IRequest<UpdateAppApiTokenCommand>
    {
        public Guid AppId { get; set; }
    }
}