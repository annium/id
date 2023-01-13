using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Apps;

namespace Server.ViewModels.Requests.Apps;

public class UpdateAppRequest : UpdateAppRequestBody, IRequest<UpdateAppCommand>
{
    public Guid AppId { get; set; }
}

public class UpdateAppRequestBody
{
    public string Name { get; set; } = string.Empty;
}