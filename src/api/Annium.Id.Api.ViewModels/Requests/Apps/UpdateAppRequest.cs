using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Apps;

namespace Annium.Id.Api.ViewModels.Requests.Apps;

public class UpdateAppRequest : UpdateAppRequestBody, IRequest<UpdateAppCommand>
{
    public Guid AppId { get; set; }
}

public class UpdateAppRequestBody
{
    public string Name { get; set; } = string.Empty;
}