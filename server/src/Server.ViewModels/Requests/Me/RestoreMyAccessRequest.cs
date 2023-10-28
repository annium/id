using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Me;

namespace Server.ViewModels.Requests.Me;

public record RestoreMyAccessRequest : RestoreMyAccessRequestBody, IRequest<RestoreMyAccessCommand>
{
    public Guid AppId { get; set; }
}

public record RestoreMyAccessRequestBody
{
    public string Server { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
