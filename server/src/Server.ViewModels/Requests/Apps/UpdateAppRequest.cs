using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Apps;

namespace Server.ViewModels.Requests.Apps;

public record UpdateAppRequest : UpdateAppRequestBody, IRequest<UpdateAppCommand>
{
    public Guid AppId { get; set; }
}

public record UpdateAppRequestBody
{
    public string Name { get; set; } = string.Empty;
}