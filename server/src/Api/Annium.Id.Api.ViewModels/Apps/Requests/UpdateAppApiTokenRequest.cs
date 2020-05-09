using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Apps;

namespace Annium.Id.Api.ViewModels.Apps.Requests
{
    public class UpdateAppApiTokenRequest : IRequest<UpdateAppApiTokenCommand>
    {
        public Guid AppId { get; set; }
    }
}