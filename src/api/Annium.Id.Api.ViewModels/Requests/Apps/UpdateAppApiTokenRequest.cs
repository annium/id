using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Apps;

namespace Annium.Id.Api.ViewModels.Requests.Apps;

public class UpdateAppApiTokenRequest : IRequest<UpdateAppApiTokenCommand>
{
    public Guid AppId { get; set; }
}