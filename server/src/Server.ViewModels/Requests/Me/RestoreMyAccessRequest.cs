using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Me;

namespace Server.ViewModels.Requests.Me;

public class RestoreMyAccessRequest : RestoreMyAccessRequestBody, IRequest<RestoreMyAccessCommand>
{
    public Guid AppId { get; set; }
}

public class RestoreMyAccessRequestBody
{
    public string Server { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}