using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Apps;

namespace Server.ViewModels.Requests.Apps;

public class UpdateAppApiTokenRequest : IRequest<UpdateAppApiTokenCommand>
{
    public Guid AppId { get; set; }
}