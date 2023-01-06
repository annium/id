using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Me;

namespace Annium.Id.Api.ViewModels.Requests.Me;

public class RestoreMyAccessRequest : RestoreMyAccessRequestBody, IRequest<RestoreMyAccessCommand>
{
    public Guid AppId { get; set; }
}

public class RestoreMyAccessRequestBody
{
    public string Server { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}